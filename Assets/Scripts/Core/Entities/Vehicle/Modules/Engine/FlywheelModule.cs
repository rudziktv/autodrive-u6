using Core.Entities.Vehicle.Data.Drivetrain.Engine;
using UnityEngine;

namespace Core.Entities.Vehicle.Modules.Engine
{
    public class FlywheelModule : VehicleModule
    {
        private CombustionEngineData _data;
        public float RPM { get; private set; }
        public float FlywheelInertiaMoment => _data.flywheelWeight * Mathf.Pow(_data.flywheelRadius, 2) * 0.5f;

        public FlywheelModule(CombustionEngineData data, VehicleController ctr) : base(ctr)
        {
            _data = data;
        }

        public override void FixedUpdateModule()
        {
            base.FixedUpdateModule();
        }

        public void TransferEngineTorque(float engineTorque)
        {
            RPM += AngleVelocityToRPM(engineTorque * _data.flywheelFreeMotionFactor / FlywheelInertiaMoment) * Time.fixedDeltaTime;
            RPM = Mathf.Clamp(RPM, 0, float.MaxValue);
        }

        public float EngineTorqueToDeltaRPM(float engineTorque)
        {
            return AngleVelocityToRPM(engineTorque * _data.flywheelFreeMotionFactor / FlywheelInertiaMoment) *
                   Time.fixedDeltaTime;
        }

        public float DeltaRPMToEngineTorque(float deltaRPM)
            => deltaRPM / 60f * 2f * Mathf.PI * FlywheelInertiaMoment / Time.fixedDeltaTime;
        
        
        private float AngleVelocityToRPM(float angleVel)
            => angleVel / (2f * Mathf.PI) * 60f;
    }
}