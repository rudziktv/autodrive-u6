using Plugins.UnityFFB.NativeEffects;
using Plugins.UnityFFB.Structures;

namespace Plugins.UnityFFB
{
    public class HapticDevice
    {
        private FFBDeviceInfo _deviceInfo;

        public uint HapticID => _deviceInfo.ID;
        public string HapticName => _deviceInfo.Name;

        private int _effectID = 0;

        public HapticDevice(FFBDeviceInfo deviceInfo)
        {
            _deviceInfo = deviceInfo;
        }
        
        public HapticDevice(uint hapticID, string hapticName)
        {
            _deviceInfo = new FFBDeviceInfo
            {
                ID = hapticID,
                Name = hapticName
            };
        }

        public void OpenHaptic()
        {
            NativeSDL3.OpenHaptic(HapticID);
        }

        public void CloseHaptic()
        {
            NativeSDL3.CloseHaptic(HapticID);
        }

        public void HapticPeriodicEffect(uint length, ushort period, short magnitude)
        {
            // Marshal.StructureToPtr();
            
            // NativeSDL3.HapticPeriodicEffect(length, period, magnitude);
        }

        public void HapticSpringEffect(uint length, ushort deadband, short leftCoefficient, short rightCoefficient, ushort leftSaturation, ushort rightSaturation)
        {
            var spring = new HapticCondition
            {
                type = Effects.HAPTIC_SPRING,
                direction =
                {
                    type = Directions.HAPTIC_POLAR,
                    dir = new [] { 0, 0, 0 }
                },
                length = length, // 5000
                // deadband = new[] { deadband }, // 0
                // left_coeff = new[] { leftCoefficient }, // 10000
                // right_coeff = new[] { rightCoefficient }, // 10000
                // left_sat = new[] { leftSaturation }, // 10000
                // right_sat = new[] { rightSaturation } // 10000
            }
            .SetDeadband0(deadband)
            .SetLeftCoeff0(leftCoefficient)
            .SetRightCoeff0(rightCoefficient)
            .SetLeftSat0(leftSaturation)
            .SetRightSat0(rightSaturation);
            
            NativeSDL3.UpdateOrCreateConditionEffect(HapticID, ref _effectID, ref spring);
        }

        #region Cast

        public static implicit operator FFBDeviceInfo(HapticDevice hapticDevice)
        {
            return hapticDevice._deviceInfo;
        }

        public static implicit operator HapticDevice(FFBDeviceInfo deviceInfo)
        {
            return new HapticDevice(deviceInfo);
        }

        public static implicit operator uint(HapticDevice hapticDevice)
        {
            return hapticDevice.HapticID;
        }

        public static implicit operator string(HapticDevice hapticDevice)
        {
            return hapticDevice.HapticName;
        }
        
        #endregion
    }
}