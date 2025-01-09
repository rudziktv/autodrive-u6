using System;
using UnityEngine;

namespace Core.Entities.Vehicle.Subentities.Lights
{
    [Serializable]
    public class EmissionMeshPair
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private float emissionIntensity;
        [SerializeField] private Color emissionColor;
        [SerializeField] private int materialIndex;

        public EmissionMeshPair() { }

        public EmissionMeshPair(MeshRenderer meshRenderer, float emissionIntensity, Color color, int materialIndex)
        {
            emissionColor = color;
            this.meshRenderer = meshRenderer;
            this.emissionIntensity = emissionIntensity;
            this.materialIndex = materialIndex;
        }
        
        public MeshRenderer MeshRenderer => meshRenderer;
        public float EmissionIntensity => emissionIntensity;
        public Color EmissionColor => emissionColor;
        public int MaterialIndex => materialIndex;

        public void ApplyEmission()
        {
            
        }
    }
}