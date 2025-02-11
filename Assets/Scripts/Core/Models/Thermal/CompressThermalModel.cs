using UnityEngine;

namespace Core.Models.Thermal
{
    public class CompressThermalModel : ThermalModel
    {
        private float _compressionRatio;
        public float DiabeticIndex { get; }
        public float Density { get; }

        private float UncompressedVolume => Density * Mass;
        private float Volume => Density * Mass / CompressionRatio;

        public float CompressionRatio
        {
            get => _compressionRatio;
            set
            {
                var oldCompressionRatio = _compressionRatio;
                _compressionRatio = value;
                Temperature *= Mathf.Pow(UncompressedVolume / oldCompressionRatio / Volume, DiabeticIndex - 1f);
            }
        }

        public CompressThermalModel(float density, float diabeticIndex, float initialTemp, float mass, float specificHeat, ThermalTickUpdate tick) : base(initialTemp, mass, specificHeat, tick)
        {
            Density = density;
            DiabeticIndex = diabeticIndex;
            CompressionRatio = 1;
        }
    }
}