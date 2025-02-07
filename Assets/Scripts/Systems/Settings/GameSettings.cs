using UnityEngine;

namespace Systems.Settings
{
    public class GameSettings : MonoBehaviour
    {
        public GameSettings Instance { get; private set; }
        
        [SerializeField] private InputSettings input;
    }
}