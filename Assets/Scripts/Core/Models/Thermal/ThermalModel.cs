using System;
using Systems.Environment;
using UnityEngine;

namespace Core.Models.Thermal
{
    public class ThermalModel
    {
        /// <summary>
        /// Specific heat of the object, kJ/kg*K.
        /// </summary>
        public float SpecificHeat { get; }
        
        /// <summary>
        /// Temperature of the object, K.
        /// </summary>
        public float Temperature { get; protected set; }
        
        /// <summary>
        /// Mass of the object, kg.
        /// </summary>
        public float Mass { get; set; }
        
        // private bool
        private readonly ThermalTickUpdate _tick;

        private float Tick => _tick switch
        {
            ThermalTickUpdate.FixedUpdate => Time.fixedDeltaTime,
            ThermalTickUpdate.Update => Time.deltaTime,
            _ => 1
        };


        public ThermalModel(float initialTemp, float mass, float specificHeat, ThermalTickUpdate tick)
        {
            Temperature = initialTemp;
            SpecificHeat = specificHeat;
            Mass = mass;
            _tick = tick;
        }
        
        // public ThermalModel(float specificHeat) : this(SimpleEnvironment.instance.AmbientTemperatureKelvin, specificHeat) { }

        public void InstantConvect(ThermalModel other, float factor, float area, float multiplier = 1f)
            => other.TransferHeat(-ConvectHeat(other.Temperature - Temperature, factor, area, multiplier));
        
        public void ConvectTransferHeat(ThermalModel other, float factor, float area, float multiplier = 1f)
            => InstantConvect(other, factor, area, multiplier * Tick);

        public void ConvectHeatToAmbient(float factor, float area, float multiplier = 1f)
            => ConvectHeat(SimpleEnvironment.instance.AmbientTemperatureKelvin - Temperature, factor, area, multiplier);

        private float ConvectHeat(float deltaTemp, float convectFactor, float area, float multiplier = 1f)
        {
            var q = convectFactor * area * deltaTemp * multiplier;
            TransferHeat(q);
            return q;
        }

        private void TransferHeat(float q)
            => Temperature += q / SpecificHeat / Mass;
    }
}