using Core.Patterns.UI;
using Systems.Devices.Infotainments.RNS510.Enums;
using UnityEngine.UIElements;

namespace Systems.Devices.Infotainments.RNS510.Models
{
    public class RNS510ButtonModel : RNS510Model
    {
        public RNS510ButtonModel(UIController<RNS510> ctr) : base(ctr, "ButtonModel") { }
        
        public void RNS510ButtonClicked(RNS510Button button)
        {
            switch (button)
            {
                case RNS510Button.Power:
                    OnPowerButton();
                    break;
                case RNS510Button.VolumeUp:
                    OnVolumeUp();
                    break;
                case RNS510Button.VolumeDown:
                    OnVolumeDown();
                    break;
            }
            //
            // if (CurrentPowerState == RNS510PowerState.On)
            //     CurrentModelRNS.RNS510ButtonClicked(button);
        }

        private void OnVolumeUp() =>
            Controller.SettingsManager.VolumeUp();
        private void OnVolumeDown() =>
            Controller.SettingsManager.VolumeDown();
        
        private void OnPowerButton()
        {
            if (Controller.PowerManager == RNS510PowerState.Off)
                Controller.PowerManager.PowerOn();
            else
                Controller.PowerManager.PowerOff();
        }
    }
}