using UnityEngine;

namespace Core.Components.Lights
{
    public interface ILightLevel
    {
        public string[] GetLightNames();
        public string[] GetEmissiveNames();
        public GameObject[] GetCutOffLines();
    }
}