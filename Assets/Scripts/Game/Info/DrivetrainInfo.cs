using UnityEngine;

namespace Game.Info
{
    [CreateAssetMenu(menuName = "Vehicles/Drivetrain Info")]
    public class DrivetrainInfo : ScriptableObject
    {
        [field: SerializeField] public string DrivetrainName { get; private set; }
        [field: SerializeField] public EngineInfo Engine { get; private set; }
    }
}