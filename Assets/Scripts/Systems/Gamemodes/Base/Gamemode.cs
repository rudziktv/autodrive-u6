using UnityEngine;

namespace Systems.Gamemodes.Base
{
    public class Gamemode : MonoBehaviour
    {
        [field: SerializeField] public string Tag { get; protected set; }
        [field: SerializeField] public GameObject CameraPrefab { get; protected set; }
        
        public virtual void Initialize() { }
        public virtual void Initialize(Transform spawnPoint) { }
        public virtual void Transition(Transform oldCameraTransform) { }
    }
}