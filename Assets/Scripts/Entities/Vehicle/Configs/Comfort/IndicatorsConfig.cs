using System;
using Entities.Vehicle.Subentities.Dashboard;
using UnityEngine;

namespace Entities.Vehicle.Configs.Comfort
{
    [Serializable]
    public class IndicatorsConfig
    {
        [SerializeField] private IndicatorController batteryIndicator;
        [SerializeField] private IndicatorController checkEngineIndicator;
        [SerializeField] private IndicatorController blinkerLeftIndicator;
        [SerializeField] private IndicatorController blinkerRightIndicator;
        [SerializeField] private IndicatorController ecuIndicator;
        [SerializeField] private IndicatorController hotCoolantIndicator;
        [SerializeField] private IndicatorController lowFuelIndicator;
        [SerializeField] private IndicatorController absIndicator;
        [SerializeField] private IndicatorController asrIndicator;
        [SerializeField] private IndicatorController asrOffIndicator;
        [SerializeField] private IndicatorController airbagIndicator;
        [SerializeField] private IndicatorController parkingBrakeIndicator;
        [SerializeField] private IndicatorController seatbeltIndicator;
        [SerializeField] private IndicatorController brakeIndicator;
        [SerializeField] private IndicatorController tpmsIndicator;
        [SerializeField] private IndicatorController steeringIndicator;

        [SerializeField] private IndicatorController automaticLightsIndicator;
        [SerializeField] private IndicatorController positionLightsIndicator;
        
        // [SerializeField] private IlluminatedIndicatorController
        
        public IndicatorController BatteryIndicator => batteryIndicator;
        public IndicatorController CheckEngineIndicator => checkEngineIndicator;
        public IndicatorController BlinkerLeftIndicator => blinkerLeftIndicator;
        public IndicatorController BlinkerRightIndicator => blinkerRightIndicator;
        public IndicatorController EcuIndicator => ecuIndicator;
        public IndicatorController HotCoolantIndicator => hotCoolantIndicator;
        public IndicatorController LowFuelIndicator => lowFuelIndicator;
        public IndicatorController AbsIndicator => absIndicator;
        public IndicatorController AsrIndicator => asrIndicator;
        public IndicatorController AsrOffIndicator => asrOffIndicator;
        public IndicatorController AirbagIndicator => airbagIndicator;
        public IndicatorController ParkingBrakeIndicator => parkingBrakeIndicator;
        public IndicatorController SeatbeltIndicator => seatbeltIndicator;
        public IndicatorController BrakeIndicator => brakeIndicator;
        public IndicatorController TpmsIndicator => tpmsIndicator;
        public IndicatorController SteeringIndicator => steeringIndicator;
        public IndicatorController AutomaticLightsIndicator => automaticLightsIndicator;
        public IndicatorController PositionLightsIndicator => positionLightsIndicator;
    }
}