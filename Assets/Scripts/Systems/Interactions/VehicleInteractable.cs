using System;
using Core.Entities.Vehicle;
using UnityEngine;

namespace Systems.Interactions
{
    public class VehicleInteractable : Interactable
    {
        protected VehicleController Vehicle;
        protected VehicleInputActions Input => Vehicle.VehicleInput;
        protected Animator Animator => Vehicle.VehicleConfigs.ComponentsReferences.Animator;

        private void Awake()
        {
            Vehicle = GetComponentInParent<VehicleController>();
        }
    }
}