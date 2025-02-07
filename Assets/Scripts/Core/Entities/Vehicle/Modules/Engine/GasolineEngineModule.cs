using Core.Entities.Vehicle.Data.Drivetrain.Engine;
using UnityEngine;

namespace Core.Entities.Vehicle.Modules.Engine
{
    public class GasolineEngineModule : CombustionEngineModule
    {
        private CombustionEngineData _data;
        private float _throttle;
        
        public float Throttle
        {
            get => _throttle;
            set => _throttle = Mathf.Clamp01(value);
        }

        public float FlywheelAngularVelocity { get; private set; }
        public float FlywheelRPM { get; private set; } = 1500f;
        // public float CurrentTorque { get; private set; }

        private float FlywheelInertiaMoment => _data.flywheelWeight * Mathf.Pow(_data.flywheelRadius, 2) * 0.5f;

        public GasolineEngineModule(CombustionEngineData data, VehicleController ctr) : base(ctr)
        {
            _data = data;
        }

        private void SuckFuel(float desiredAmount)
        {
            
        }
        
        public override void FixedUpdateModule()
        {
            base.FixedUpdateModule();

            var input = VehicleInput.Drive.GasPedal.ReadValue<float>();

            // temporary holding idle rpm
            Throttle = FlywheelRPM < 800f && input == 0 ? 0.1f : input;
            
            Cycle();

            Debug.Log($"RPM: {FlywheelRPM}, Thr: {Throttle}, Inertia: {FlywheelInertiaMoment}");
            ComfortConfig.DashboardConfig.Tachometer.Value = Mathf.Clamp(FlywheelRPM, 0f, 10000f);
        }

        private void Cycle()
        {
            var currentTorque = CurrentTorque;
            var drag = _data.innerDrag + CurrentInnerRevDrag + CurrentTorqueBasedDrag;
            FlywheelRPM += AngleVelocityToRPM((currentTorque - drag) / FlywheelInertiaMoment) * Time.fixedDeltaTime;
        }

        // private float CurrentMaxTorque =>
            // _data.torqueCurve.Evaluate(FlywheelRPM / _data.torqueRevScale) * _data.maxTorque;
        
        private float CurrentTorque =>
            _data.torqueCurve.Evaluate(FlywheelRPM / _data.torqueRevScale) * Throttle * _data.maxTorque;

        private float CurrentInnerRevDrag =>
            _data.innerRevDrag * _data.innerRevDragCurve.Evaluate(FlywheelRPM / _data.torqueRevScale);
        
        private float CurrentTorqueBasedDrag =>
            _data.torqueBasedDrag * _data.torqueCurve.Evaluate(FlywheelRPM / _data.torqueRevScale);

        private float AngleVelocityToRPM(float angleVel)
            => angleVel / (2f * Mathf.PI) * 60f;
    }
}