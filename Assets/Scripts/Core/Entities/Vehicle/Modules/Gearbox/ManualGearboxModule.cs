using Core.Entities.Vehicle.Data.Drivetrain;
using Core.Entities.Vehicle.Data.Drivetrain.Gearbox;
using Core.Entities.Vehicle.Modules.Engine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Entities.Vehicle.Modules.Gearbox
{
    public class ManualGearboxModule : GearboxModule
    {
        private int _selectedGear;
        private VehicleManualGearboxData Data { get; }
        
        public float ClutchInput { get; private set; }
        public float InvertedClutchInput { get; private set; }

        public float ClutchCurve => Data.clutchResponseCurve.Evaluate(ClutchInput);
        public float InvertedClutchCurve => Data.clutchResponseCurve.Evaluate(InvertedClutchInput);
        
        public float ClutchRPM => OutputRPM * CurrentGearRatio * Data.finalRatio;
        public float ClutchAngularVelocity => ClutchRPM / 60f * 2f * Mathf.PI;
        
        public override float OutputRPM { get; protected set; }

        private float CurrentGearRatio => SelectedGear switch
        {
            0 => 0f,
            -1 => Data.reverseRatio,
            _ => Data.gearRatios[SelectedGear - 1]
        };

        public float OutputTorque { get; private set; }

        public GasolineEngineModule Engine => (GasolineEngineModule)Drivetrain.Engine;

        public int SelectedGear
        {
            get => _selectedGear;
            private set => _selectedGear = Mathf.Clamp(value, -1, Data.gearRatios.Length);
        }

        public ManualGearboxModule(VehicleManualGearboxData data, VehicleController ctr) : base(ctr)
        {
            Data = data;
        }

        public override void Initialize()
        {
            base.Initialize();
            SetupGearInputEvents();
        }

        public override void FixedUpdateModule()
        {
            base.FixedUpdateModule();
            UpdateInputs();
            UpdateClutch();
            UpdateGearbox();
        }

        private void UpdateInputs()
        {
            ClutchInput = VehicleInput.Drive.ClutchPedal.ReadValue<float>();
            InvertedClutchInput = 1f - ClutchInput;
        }

        private void UpdateClutch()
        {
            var clutchPressure = Data.clutchMaxPressure * InvertedClutchCurve;
            var maxClutchTorque = Data.clutchFrictionFactor * clutchPressure * Data.clutchRadius;
            var c = maxClutchTorque / 50f;

            var deltaAngularVelocity = Engine.Flywheel.AngularVelocity - ClutchAngularVelocity;

            var torque = Mathf.Min(maxClutchTorque, c * Mathf.Pow(Mathf.Abs(deltaAngularVelocity), Data.power) + Engine.CurrentTorque) *
                         Mathf.Sign(deltaAngularVelocity);
            
            var engineLossTorque = Mathf.Min(maxClutchTorque, c * Mathf.Pow(Mathf.Abs(deltaAngularVelocity), Data.power)) *
                         Mathf.Sign(deltaAngularVelocity);


            Engine.Flywheel.DrivetrainConnected = Engine.Flywheel.AngularVelocity - ClutchAngularVelocity < Data.connectionDifference
                && SelectedGear != 0 && ClutchInput > 0f;

            if (SelectedGear != 0)
            {
                // Engine.Flywheel.TransferEngineTorque(-engineLossTorque);

                if (Engine.Flywheel.DrivetrainConnected)
                    Engine.Flywheel.ConnectGearboxRPM(ClutchRPM);
                else
                    Engine.Flywheel.TransferEngineTorque(-engineLossTorque);

                
                OutputTorque = (Engine.Flywheel.DrivetrainConnected ? Engine.CurrentTorque : torque) * CurrentGearRatio * Data.finalRatio;
            }
            
            // Debug.Log($"FlywheelRPM: {Engine.FlywheelRPM} Current Gear: {SelectedGear}, Clutch: {ClutchInput}, Max Clutch Torque: {maxClutchTorque}, Clutch RPM: {ClutchRPM}, Output RPM: {OutputRPM}, Output Torque: {OutputTorque}, Connected: {Engine.Flywheel.DrivetrainConnected}");
        }

        private void UpdateGearbox()
        {
            OutputRPM = Drivetrain.TransferCase.CurrentRPM;
            Drivetrain.TransferCase.TransferTorque(OutputTorque);
        }

        private void SetGear(int gear)
        {
            if (gear == 0)
            {
                SelectedGear = 0;
                return;
            }

            UpdateInputs();
            if (ClutchInput >= 0.95f)
                SelectedGear = gear;
        }

        public override bool IsNeutral()
            => SelectedGear == 0;

        #region GearInputEvents

        private void SetupGearInputEvents()
        {
            VehicleInput.Shifter.Reverse.started += GearRSelected;
            VehicleInput.Shifter.Gear1.started += Gear1StSelected;
            VehicleInput.Shifter.Gear2.started += Gear2NdSelected;
            VehicleInput.Shifter.Gear3.started += Gear3RdSelected;
            VehicleInput.Shifter.Gear4.started += Gear4ThSelected;
            VehicleInput.Shifter.Gear5.started += Gear5ThSelected;
            VehicleInput.Shifter.Gear6.started += Gear6ThSelected;
            
            VehicleInput.Shifter.Reverse.canceled += GearNSelected;
            VehicleInput.Shifter.Gear1.canceled += GearNSelected;
            VehicleInput.Shifter.Gear2.canceled += GearNSelected;
            VehicleInput.Shifter.Gear3.canceled += GearNSelected;
            VehicleInput.Shifter.Gear4.canceled += GearNSelected;
            VehicleInput.Shifter.Gear5.canceled += GearNSelected;
            VehicleInput.Shifter.Gear6.canceled += GearNSelected;
        }

        private void GearRSelected(InputAction.CallbackContext ctx)
            => SetGear(-1);

        private void GearNSelected(InputAction.CallbackContext ctx)
            => SetGear(0);

        private void Gear1StSelected(InputAction.CallbackContext obj)
            => SetGear(1);
        
        private void Gear2NdSelected(InputAction.CallbackContext obj)
            => SetGear(2);
        
        private void Gear3RdSelected(InputAction.CallbackContext obj)
            => SetGear(3);
        
        private void Gear4ThSelected(InputAction.CallbackContext obj)
            => SetGear(4);
        
        private void Gear5ThSelected(InputAction.CallbackContext obj)
            => SetGear(5);
        
        private void Gear6ThSelected(InputAction.CallbackContext obj)
            => SetGear(6);
        
        private void Gear7ThSelected(InputAction.CallbackContext obj)
            => SetGear(7);

        #endregion
    }
}