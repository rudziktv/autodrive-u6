using Systems.Info;
using UnityEngine;

namespace Core.Entities.Vehicle.Data.Drivetrain.Engine
{
    [CreateAssetMenu(fileName = "VehicleEngineData", menuName = "Vehicle/Engine", order = 0)]
    public class VehicleEngineData : ScriptableObject
    {
        [Header("Info")]
        public string engineName;
        public string engineCode;
        public EngineType engineType;
        
        [Header("Torque")]
        public float maxTorque;
        public float torqueRevScale;
        public AnimationCurve torqueCurve;
    }
}