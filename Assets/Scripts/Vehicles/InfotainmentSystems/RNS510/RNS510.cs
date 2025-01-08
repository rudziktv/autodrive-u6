using System;
using UnityEngine;
using UnityEngine.UIElements;
using Vehicles.InfotainmentSystems.RNS510.ViewModels;

namespace Vehicles.InfotainmentSystems.RNS510
{
    [RequireComponent(typeof(UIDocument))]
    public class RNS510 : MonoBehaviour
    {
        [SerializeField] private UIDocument rnsUi;
        [field: SerializeField] public RNS510Assets Assets { get; private set; }

        private RNS510Controller _rnsController;

        private void Start()
        {
            _rnsController = new RNS510Controller(this, rnsUi.rootVisualElement);
            _rnsController.NavigateTo(new RNS510RadioViewModel(_rnsController,
                Assets.GetRadioScreenByLoad().Instantiate()));
        }
    }
}