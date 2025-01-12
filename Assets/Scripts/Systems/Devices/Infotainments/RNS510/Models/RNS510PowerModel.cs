using Core.Entities.Vehicle.Enums;
using Core.Patterns.UI;
using Systems.Devices.Infotainments.RNS510.Enums;

namespace Systems.Devices.Infotainments.RNS510.Models
{
    public class RNS510PowerModel : RNS510Model
    {
        public RNS510PowerState CurrentPowerState { get; set; }
        
        public RNS510PowerModel(UIController<RNS510> ctr) : base(ctr, "PowerModel") { }

        public void OnElectricityStateChanged(ElectricityState state)
        {
            switch (state)
            {
                case ElectricityState.Off:
                    break;
                case ElectricityState.OnlyAccessories:
                    break;
                case ElectricityState.LowPowerMode:
                    break;
                case ElectricityState.Ignition:
                    PowerOn();
                    break;
                case ElectricityState.Engine:
                    break;
            }
            //
            // CurrentModelRNS.OnElectricityStateChanged(state);
        }

        public void PowerOn()
        {
            if (CurrentPowerState == RNS510PowerState.Off)
            {
                CurrentPowerState = RNS510PowerState.SplashScreen;
                Controller.SplashScreen();
            }
        }

        public void PowerOff()
        {
            
        }

        public static implicit operator RNS510PowerState(RNS510PowerModel model)
        {
            return model.CurrentPowerState;
        }
    }
}