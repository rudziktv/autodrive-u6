using UnityEngine;

namespace Systems.Environment
{
    public class SimpleEnvironment : MonoBehaviour
    {
        public static SimpleEnvironment instance;
        
        public float currentTemperature = 0f;

        private void Awake()
        {
            instance = this;
        }
    }
}