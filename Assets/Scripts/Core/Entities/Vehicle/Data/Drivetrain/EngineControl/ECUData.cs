using UnityEngine;

namespace Core.Entities.Vehicle.Data.Drivetrain.EngineControl
{
    [CreateAssetMenu(fileName = "ECU", menuName = "Vehicle/Engine/Control/ECU", order = 0)]
    public class ECUData : ScriptableObject
    {
        [Header("Idle")]
        public float idleRev;
        public float idleWarmUpRev;
        public float warmUpRevMinTemp;
        public float warmUpRevMaxTemp;

        [Header("Rev Limiter")]
        public bool neutralRevLimiterEnabled;
        public float neutralRevLimiter;
    }
}