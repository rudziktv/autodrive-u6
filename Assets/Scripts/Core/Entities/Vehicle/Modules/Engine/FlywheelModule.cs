using Core.Entities.Vehicle.Data.Drivetrain;
using Core.Entities.Vehicle.Data.Drivetrain.Engine;
using UnityEngine;

namespace Core.Entities.Vehicle.Modules.Engine
{
    public class FlywheelModule : DrivetrainSubmodule
    {
        private CombustionEngineData _data;
        public float RPM { get; private set; }
        
        /// <summary>
        /// Angular velocity of the flywheel, rad/s
        /// </summary>
        public float AngularVelocity => RPM / 60f * 2f * Mathf.PI;
        public float FlywheelInertiaMoment => _data.flywheelWeight * Mathf.Pow(_data.flywheelRadius, 2) * 0.5f;

        public bool DrivetrainConnected { get; set; }

        public FlywheelModule(CombustionEngineData data, VehicleController ctr) : base(ctr)
        {
            _data = data;
        }

        public override void FixedUpdateModule()
        {
            base.FixedUpdateModule();
            
            // if ()
        }

        public void TransferEngineTorque(float engineTorque)
        {
            if (DrivetrainConnected) return;
            RPM += AngleVelocityToRPM(engineTorque * _data.flywheelFreeMotionFactor / FlywheelInertiaMoment) * Time.fixedDeltaTime;
            RPM = Mathf.Clamp(RPM, 0, float.MaxValue);
        }

        public void ConnectGearboxRPM(float newRPM)
        {
            RPM = newRPM;
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