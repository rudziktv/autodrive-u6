using UnityEngine;

namespace Systems.Info
{
    [CreateAssetMenu(menuName = "Vehicles/Engine Info")]
    public class EngineInfo : ScriptableObject
    {
        [field: SerializeField] public string EngineName { get; private set; }
        [field: SerializeField] public float MaxTorque { get; private set; }
        [field: SerializeField] public AnimationCurve TorqueCurve { get; private set; }
      
        [field: SerializeField] public FuelTypeEnum FuelType { get; private set; }
        
        [Header("RPM")]
        [field: SerializeField] public float MaxRev { get; private set; }
        [field: SerializeField] public float IdleRev { get; private set; }
        [field: SerializeField] public float MinRev { get; private set; }
        [field: SerializeField] public float CutOffRev { get; private set; }
        [field: SerializeField] public bool CutOff { get; private set; }
    }
}