using UnityEngine;

namespace Systems.Gamemodes.Base
{
    public class Gamemode : MonoBehaviour
    {
        public virtual void Initialize() { }
        public virtual void Initialize(Transform cameraTransform) { }
    }
}