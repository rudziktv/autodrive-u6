using UnityEngine;

namespace Graphics.Lights
{
    public interface ILightLevel
    {
        public string[] GetLightNames();
        public string[] GetEmissiveNames();
        public GameObject[] GetCutOffLines();
    }
}