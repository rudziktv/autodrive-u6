using System;
using Entities.Vehicle.Configs.Electricity;
using Entities.Vehicle.Enums;
using Entities.Vehicle.Submodules.Electricity;

namespace Entities.Vehicle.Managers
{
    public class ElectricityManager : VehicleManager
    {
        public ElectricityState CurrentElectricityState { get; private set; } = ElectricityState.Off;
        
        public event ElectricityStateChangedArgs OnElectricityStateChanged;
        public delegate void ElectricityStateChangedArgs(ElectricityState newState);
        
        public Battery Battery { get; }
        public Alternator Alternator { get; }
        public float CurrentLoad { get; private set; } = 0f;

        public ElectricityManager(VehicleController controller) : base(controller)
        {
            Battery = new(Controller);
            Alternator = new(Controller);
            
            Interactions.KeyIgnition.OnStateChanged += OnKeyPositionChanged;
            
            ChangeElectricityState(ElectricityState.Off);
        }

        private void OnKeyPositionChanged(KeyPositionState keyState, KeyPositionState oldState)
        {
            switch (keyState)
            {
                case KeyPositionState.NoKey:
                    ChangeElectricityState(ElectricityState.Off);
                    break;
                case KeyPositionState.Off:
                    if (oldState == KeyPositionState.NoKey)
                        break;
                    ChangeElectricityState(ElectricityState.OnlyAccessories);
                    break;
                case KeyPositionState.Ignition:
                    ChangeElectricityState(ElectricityState.Ignition);
                    break;
                case KeyPositionState.Running:
                    // depends on engine started properly
                    break;
                case KeyPositionState.Starter:
                    ChangeElectricityState(ElectricityState.LowPowerMode);
                    break;
            }
        }

        public void ChangeElectricityState(ElectricityState newState)
        {
            CurrentElectricityState = newState;
            OnElectricityStateChanged?.Invoke(CurrentElectricityState);
        }

        public void Update()
        {
            Battery.UpdateModule();
            Alternator.UpdateModule();
            
            var currentFromAlternator = (Alternator.OutputVoltage - Battery.CurrentVoltage) / Battery.Config.InternalResistance;
            var currentToBattery = currentFromAlternator - CurrentLoad; // it can either charge or discharge the battery
            
            // Load on battery
            Battery.LoadOnBattery(currentToBattery);
        }

        public void LoadChange(float delta)
        {
            CurrentLoad += delta;
        }
    }
}