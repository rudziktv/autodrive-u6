using System;
using Systems.Environment;
using UnityEngine;

namespace Core.Models.Thermal
{
    public class ThermalModel
    {
        /// <summary>
        /// Thermal capacity of the object, kJ/kg*K.
        /// </summary>
        public float ThermalCapacity { get; }
        
        /// <summary>
        /// Temperature of the object, K.
        /// </summary>
        public float Temperature { get; set; }
        
        /// <summary>
        /// Mass of the object, kg.
        /// </summary>
        public float Mass { get; set; }
        
        private readonly ThermalTickUpdate _tick;

        private float Tick => _tick switch
        {
            ThermalTickUpdate.FixedUpdate => Time.fixedDeltaTime,
            ThermalTickUpdate.Update => Time.deltaTime,
            _ => 1
        };

        public ThermalModel(float mass, float thermalCapacity, ThermalTickUpdate tick = ThermalTickUpdate.FixedUpdate) :
            this(SimpleEnvironment.instance.AmbientTemperatureKelvin, mass, thermalCapacity, tick) { }
        
        public ThermalModel(float initialTemp, float mass, float thermalCapacity, ThermalTickUpdate tick = ThermalTickUpdate.FixedUpdate)
        {
            Temperature = initialTemp;
            ThermalCapacity = thermalCapacity;
            Mass = mass;
            _tick = tick;
        }
        
        // public ThermalModel(float specificHeat) : this(SimpleEnvironment.instance.AmbientTemperatureKelvin, specificHeat) { }
        public void TransferHeat(float q)
            => Temperature += q / ThermalCapacity / Mass;
        
        public void InstantConvect(ThermalModel other, float factor, float area, float multiplier = 1f)
            => other.TransferHeat(-ConvectHeat(other.Temperature - Temperature, factor, area, multiplier));
        
        public void ConvectTransferHeat(ThermalModel other, float factor, float area, float multiplier = 1f)
            => InstantConvect(other, factor, area, multiplier * Tick);

        public void ConvectHeatToAmbient(float factor, float area, float multiplier = 1f)
            => ConvectHeat(SimpleEnvironment.instance.AmbientTemperatureKelvin - Temperature, factor, area, Tick * multiplier);
        
        public void InstantConvectHeatToAmbient(float factor, float area, float multiplier = 1f)
            => ConvectHeat(SimpleEnvironment.instance.AmbientTemperatureKelvin - Temperature, factor, area, multiplier);

        private float ConvectHeat(float deltaTemp, float convectFactor, float area, float multiplier = 1f)
        {
            var q = convectFactor * area * deltaTemp * multiplier;
            TransferHeat(q);
            return q;
        }
    }
}