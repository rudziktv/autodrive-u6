using System;
using Core.Components.GUI.WrapperUV;
using Core.Entities.Vehicle;
using GUI.Utils;
using Systems.Devices.Infotainments.RNS510.Enums;
using Systems.Devices.Infotainments.RNS510.ViewModels;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Devices.Infotainments.RNS510
{
    [RequireComponent(typeof(UIDocument))]
    public class RNS510 : MonoBehaviour
    {
        [SerializeField] private UIDocument rnsUi;
        [field: SerializeField] public RNS510Assets Assets { get; private set; }
        [field: SerializeField] public VehicleController Vehicle { get; private set; }

        private RNS510Controller _rnsController;

        private void Start()
        {

            
            rnsUi.panelSettings.SetScreenToPanelSpaceFunction((screenPos) =>
            {
                var invalidPosition = new Vector2(float.NaN, float.NaN);
            
                if (Camera.main == null)
                    return invalidPosition;
            
                var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            
                if (!Physics.Raycast(cameraRay, out var hit, 100f, LayerMask.GetMask("UI")))
                {
                    return invalidPosition;
                }
                
                if (!hit.collider.TryGetComponent<UVCollider>(out var uv))
                    return invalidPosition;

                var point = uv.GetUV(hit.point);
                
                point.x *= rnsUi.panelSettings.targetTexture.width;
                point.y *= rnsUi.panelSettings.targetTexture.height;
                
                return point;
            });
            

            
            _rnsController = new RNS510Controller(this, rnsUi.rootVisualElement);
            _rnsController.NavigateTo(new RNS510RadioViewModel(_rnsController,
                Assets.GetRadioScreenByLoad().Instantiate()));
        }
        
        public void OnButtonClicked(int button)
        {
            _rnsController.RNS510ButtonClicked((RNS510Button)button);
        }

        private void OnDestroy()
        {
            _rnsController.Dispose();
        }
    }
}