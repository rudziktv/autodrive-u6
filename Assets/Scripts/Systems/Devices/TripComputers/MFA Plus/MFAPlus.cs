using Core.Entities.Vehicle;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Devices.TripComputers.MFA_Plus
{
    public class MFAPlus : MonoBehaviour
    {
        [SerializeField] private UIDocument document;
        [SerializeField] private VehicleController vehicleController;
        [SerializeField] private MFAPlusAssets assets;

        private MFAPlusController _controller;

        public MFAPlusAssets Assets => assets;
        public InputActions Actions { get; private set; }
        public MFAPlusComputer Computer { get; private set; }
        public VehicleController Vehicle => vehicleController;

        private void Awake()
        {
            Actions = new();
        }

        private void OnEnable()
        {
            Actions.Enable();
        }

        private void OnDisable()
        {
            Actions.Disable();
        }

        private void Start()
        {
            Computer = new();
            _controller = new(this, document.rootVisualElement);
            // _controller.NavigateTo(new MFAPlusSettingsPageModel(_controller, assets.MainScreen.Instantiate()))
            _controller.NavigateTo(new MFAPlusModel(_controller, assets.MainScreen.Instantiate(), "SplashScreen"));
        }

        private void Update()
        {
            Computer.OnUpdate();
            _controller.OnUpdate();
        }
    }
}