using System;
using Systems.Interactions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Entities.Vehicle.Interactions
{
    public class HazardsInteractable : VehicleInteractable
    {
        [SerializeField] private InteractableButton hazardsButton;

        public bool CurrentHazardsState { get; private set; }
        public event Action<bool> HazardsStateChanged;

        private void Start()
        {
            Input.Functions.Hazards.performed += OnHazardsPressed;
            hazardsButton.interact.AddListener(OnHazardsInteraction);
        }
        
        private void OnHazardsPressed(InputAction.CallbackContext ctx)
        {
            hazardsButton?.SimpleInteract();
        }

        private void InvokeHazardsEvent()
        {
            CurrentHazardsState = !CurrentHazardsState;
            HazardsStateChanged?.Invoke(CurrentHazardsState);
        }

        private void OnHazardsInteraction()
        {
            InvokeHazardsEvent();
        }
    }
}