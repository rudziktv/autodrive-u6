﻿using UnityEngine;

namespace Core.Models.Thermal
{
    public class CompressThermalModel : ThermalModel
    {
        /// <summary>
        /// Density of compressible object, kg/m³
        /// </summary>
        public float Density { get; }
        public float DiabeticIndex { get; }

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
        
        private float _compressionRatio = 1;

        public CompressThermalModel(float density, float diabeticIndex, float initialTemp,
            float mass, float thermalCapacity, ThermalTickUpdate tick = ThermalTickUpdate.FixedUpdate)
            : base(initialTemp, mass, thermalCapacity, tick)
        {
            Density = density;
            DiabeticIndex = diabeticIndex;
        }
    }
}