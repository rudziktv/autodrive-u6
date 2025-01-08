using UnityEngine;

namespace Entities.Vehicle.Subentities.Lights
{
    public class HeadlightsController : MonoBehaviour
    {
        [SerializeField] private LightLevel[] lightLevels = new LightLevel[5];
        
        public void TurnOffAllLights()
        {
            lightLevels[0].Apply(this);
        }
        
        public void TurnOnDaylights()
        {
            lightLevels[1].Apply(this);
        }

        public void TurnOnPositionLights()
        {
            lightLevels[2].Apply(this);
        }

        public void TurnOnLowBeamLights()
        {
            lightLevels[3].Apply(this);
        }

        public void TurnOnHighBeamLights()
        {
            lightLevels[4].Apply(this);
        }
    }
}