using UnityEngine;

namespace Core.Entities.Vehicle.Animations
{
    public static class InteractionVehicleAnimParams
    {
        public static readonly int LightSwitch = Animator.StringToHash("Light Switch");
        public static readonly int BlinkerStick = Animator.StringToHash("Blinker Stick");
        public static readonly int LeftComfortBlinker = Animator.StringToHash("Left Comfort Blinker");
        public static readonly int RightComfortBlinker = Animator.StringToHash("Right Comfort Blinker");
    }
}