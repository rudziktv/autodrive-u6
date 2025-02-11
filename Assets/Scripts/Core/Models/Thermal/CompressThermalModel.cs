using UnityEngine;

namespace Core.Models.Thermal
{
    public class CompressThermalModel : ThermalModel
    {
        private float _compressionRatio = 1;
        public float DiabeticIndex { get; }
        public float Density { get; }

        private float UncompressedVolume => Density * Mass;
        private float Volume => Density * Mass / CompressionRatio;

        public float CompressionRatio
        {
            get => _compressionRatio;
            set
            {
                Temperature *= Mathf.Pow(Volume / (UncompressedVolume / value)
                    , DiabeticIndex - 1f);
                _compressionRatio = value;
            }
        }

        public CompressThermalModel(float density, float diabeticIndex, float initialTemp, float mass, float thermalCapacity, ThermalTickUpdate tick) : base(initialTemp, mass, thermalCapacity, tick)
        {
            Density = density;
            DiabeticIndex = diabeticIndex;
        }
    }
}