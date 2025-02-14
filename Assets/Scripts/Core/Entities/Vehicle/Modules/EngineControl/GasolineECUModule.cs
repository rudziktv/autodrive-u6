using System.Collections;
using Core.Entities.Vehicle.Data.Drivetrain;
using Core.Entities.Vehicle.Data.Drivetrain.Engine;
using Core.Entities.Vehicle.Data.Drivetrain.EngineControl;
using Core.Entities.Vehicle.Enums;
using Core.Entities.Vehicle.Modules.Engine;
using Core.Utils;
using UnityEngine;

namespace Core.Entities.Vehicle.Modules.EngineControl
{
    public class GasolineECUModule : ECUModule
    {
        protected new GasolineEngineModule Engine =>
            GetEngineModule<GasolineEngineModule>();
        protected FlywheelModule Flywheel => Engine.Flywheel;
        
        protected ECUData Data => EngineData.ecu;
        protected CombustionEngineData EngineData { get; }

        private float idleTargetRPM = 800f;

        private float FlywheelRPM => Engine.FlywheelRPM;

        private bool _cutOff;
        private Coroutine _cutOffRoutine;
        
        public GasolineECUModule(CombustionEngineData engineData, VehicleController ctr) : base(ctr)
        {
            EngineData = engineData;
        }

        public override void FixedUpdateModule()
        {
            base.FixedUpdateModule();
            // var targetRpm = 800f;
            // var neededInput = TotalInnerDrag / CurrentMaxTorque * (targetRpm - FlywheelRPM);
            // var intelligentInput = () / CurrentMaxTorque;
            
            
            if (CurrentElectricityState != ElectricityState.Ignition)
            {
                Engine.Throttle = 0;
                return;
            }
            else
            {
                UpdateIdleTargetRPM();
                UpdateECUThrottle();
            }
            
            // Debug.Log($"ECU. ECU Thr: {Engine.Throttle} Input throttle: {VehicleInput.Drive.GasPedal.ReadValue<float>()} Brake: {VehicleInput.Drive.BrakePedal.ReadValue<float>()}");
        }

        private void UpdateECUThrottle()
        {
            var input = VehicleInput.Drive.GasPedal.ReadValue<float>();
            
            var idleThr = ThrottleIdleControl();
            // var range = 1f - idleThr;
            var ecuThr = Mathf.Lerp(idleThr, 1f, input);
            
            Engine.Throttle = ecuThr;
            
            if (Data.neutralRevLimiterEnabled && Drivetrain.Gearbox.IsNeutral())
                Engine.Throttle = RevLimiter(Data.neutralRevLimiter, Engine.Throttle, 3000f);
            Engine.Throttle = EngineData.redLineCutOff ? CutOff(EngineData.redLineRev, Engine.Throttle):
                RevLimiter(EngineData.redLineRev, Engine.Throttle);
            
            Debug.Log($"idle thr: {idleThr}, input thr: {input}, ecu thr: {ecuThr}, final thr: {Engine.Throttle}");
        }

        private void UpdateIdleTargetRPM()
        {
            var tempScale = Mathf.InverseLerp(Data.warmUpRevMaxTemp, Data.warmUpRevMinTemp, TempUnitUtils.KelvinToCelsius(Engine.OilThermal.Temperature));
            idleTargetRPM = Mathf.Lerp(Data.idleRev, Data.idleWarmUpRev, tempScale);
        }
        
        private float ThrottleIdleControl()
        {
            float deltaRpm = idleTargetRPM - FlywheelRPM;
            float torque = deltaRpm / 60f / Time.fixedDeltaTime * 2f * Mathf.PI * Engine.Flywheel.FlywheelInertiaMoment;
            float deltaTorque = torque - Engine.CurrentTorque;
            float throttle = (deltaTorque / Time.fixedDeltaTime * Engine.CurrentIntakeResponse + Engine.CurrentTorque)
                             / Engine.CurrentMaxTorque;

            float correctionFactor = 0;
            float correctionRpm = 400f;
            if (FlywheelRPM < idleTargetRPM + correctionRpm)
            {
                correctionFactor = Engine.GetTotalInnerDrag(idleTargetRPM) / Engine.GetMaxTorque(idleTargetRPM)
                                   * Mathf.Clamp01(((idleTargetRPM + correctionRpm) - FlywheelRPM) / correctionRpm);
            }
            
            
            // Debug.Log($"Δ RPM: {deltaRpm}, torque: {torque}, Δ torque: {deltaTorque}, thr: {throttle}, corr: {correctionFactor}");
            return Mathf.Clamp01(throttle + correctionFactor);
        }

        private float RevLimiter(float targetRpm, float inputThrottle, float lowPassRpmSmooth = 500f)
        {
            float deltaRpm = targetRpm - FlywheelRPM;
            float torque = deltaRpm / 60f / Time.fixedDeltaTime * 2f * Mathf.PI * Engine.Flywheel.FlywheelInertiaMoment;
            float deltaTorque = torque - Engine.CurrentTorque;
            float throttle = (deltaTorque / Time.fixedDeltaTime * Engine.CurrentIntakeResponse + Engine.CurrentTorque) / Engine.CurrentMaxTorque;
            
            float correctionFactor = Mathf.Clamp01(Engine.GetTotalInnerDrag(targetRpm) / Engine.GetMaxTorque(targetRpm));
            // float correctionRpm = 500f;
            if (FlywheelRPM < targetRpm - lowPassRpmSmooth)
                return inputThrottle;
            
            var correctionLimiter = Mathf.Lerp(inputThrottle, correctionFactor,
                Mathf.InverseLerp(targetRpm - lowPassRpmSmooth, targetRpm, FlywheelRPM));
                // Mathf.Clamp01(((targetRpm - lowPassRpmSmooth) - FlywheelRPM) / lowPassRpmSmooth));

            // checking primitive
            var iterations = 0;
            var targetTorque = 0;
            var currentTorque = Engine.CurrentTorque;
            var currentRpm = Engine.FlywheelRPM;
                
            while (currentTorque > targetTorque)
            {
                var dTorque = (targetTorque - currentTorque) / Engine.GetIntakeResponse(currentRpm) * Time.fixedDeltaTime;
                currentTorque = Mathf.Clamp(dTorque + currentTorque, float.MinValue, EngineData.maxTorque);
                if (currentTorque is > -0.1f and < 0.1f)
                    currentTorque = 0;
                currentRpm += Flywheel.EngineTorqueToDeltaRPM(currentTorque);
                iterations++;
                if (iterations > 500)
                    break;
            }

            if (currentRpm >= targetRpm)
                inputThrottle = Mathf.Clamp(inputThrottle, 0, correctionFactor);
            
            // Debug.Log($"Δ RPM: {deltaRpm}. CHECK: i: {iterations} final_rpm: {currentRpm} final_torque: {currentTorque}");

            
            // Debug.Log($"Δ RPM: {deltaRpm}, torque: {torque}, Δ torque: {deltaTorque}, thr: {throttle}, corr: {correctionFactor}, output: {correctionLimiter}");
            if (FlywheelRPM <= targetRpm)
            {
                inputThrottle = Mathf.Clamp(inputThrottle, 0, throttle);
                inputThrottle = Mathf.Clamp(inputThrottle, 0, correctionLimiter);
                return inputThrottle;
                // return Mathf.Lerp(inputThrottle, correctionFactor,
                //     Mathf.Clamp01(((targetRpm - lowPassRpmSmooth) - FlywheelRPM) / lowPassRpmSmooth));
                // Mathf.InverseLerp(targetRpm - correctionRpm, targetRpm, FlywheelRPM);
            }
            return 0f;
        }
        
        private float CutOff(float targetRpm, float inputThrottle, float lowPassRpmSmooth = 500f)
        {
            float deltaRpm = targetRpm - FlywheelRPM;
            float torque = deltaRpm / 60f / Time.fixedDeltaTime * 2f * Mathf.PI * Engine.Flywheel.FlywheelInertiaMoment;
            float deltaTorque = torque - Engine.CurrentTorque;
            float throttle = (deltaTorque / Time.fixedDeltaTime * Engine.CurrentIntakeResponse + Engine.CurrentTorque) / Engine.CurrentMaxTorque;
            
            // float correctionRpm = 500f;
            if (FlywheelRPM < targetRpm - lowPassRpmSmooth)
                return _cutOff ? 0 : inputThrottle;
            
            if (FlywheelRPM < targetRpm - targetRpm * 0.01f)
            {
                inputThrottle = Mathf.Clamp(inputThrottle, 0, throttle);
                return _cutOff ? 0 : inputThrottle;
            }

            StopCoroutine(_cutOffRoutine);
            StartCoroutine(CutOffTimer());
            _cutOff = true;
            return 0;
        }

        private IEnumerator CutOffTimer()
        {
            yield return new WaitForSeconds(EngineData.redLineCutOffTime);
            _cutOff = false;
        }
    }
}