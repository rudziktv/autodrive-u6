using Core.Patterns.UI;
using FMODUnity;
using Systems.Devices.Infotainments.RNS510.Helpers;
using Systems.Sounds.FMODProject;
using UnityEngine;

namespace Systems.Devices.Infotainments.RNS510.Models
{
    public class RNS510SettingsModel : RNS510Model
    {
        public RNS510Settings Settings { get; private set; } = new();
        
        public RNS510SettingsModel(UIController<RNS510> ctr) : base(ctr, "SettingsModel") { }
        

        public void VolumeUp()
        {
            Settings.currentVolume = Mathf.Clamp(Settings.currentVolume + 1, 0, RNS510Parameters.MAX_VOLUME);
            UpdateFMODParameters();
        }
        
        public void VolumeDown()
        {
            Settings.currentVolume = Mathf.Clamp(Settings.currentVolume - 1, 0, RNS510Parameters.MAX_VOLUME);
            UpdateFMODParameters();
        }

        public void UpdateFMODParameters()
        {
            RuntimeManager.StudioSystem.setParameterByName
                (FMODGlobalParameters.FMOD_VEHICLE_SPEAKERS_VOLUME, Settings.currentVolume / (float)RNS510Parameters.MAX_VOLUME);
        }
    }
}