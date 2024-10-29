using UnityEngine;
using UnityEngine.UIElements;
using Resources.Bundles.Cars.Volkswagen_Golf_Mk6.MFA_Plus.Models;

namespace Resources.Bundles.Cars.Volkswagen_Golf_Mk6.MFA_Plus
{
    public class MFAPlus : MonoBehaviour
    {
        [SerializeField]
        private UIDocument document;
        [SerializeField]
        private MFAPlusAssets assets;

        private MFAPlusController _controller;

        public MFAPlusAssets Assets => assets;
        public InputActions Actions { get; private set; }
        public MFAPlusComputer Computer { get; private set; }

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
            _controller.NavigateTo(new MFAPlusSettingsPageModel(_controller, assets.MainScreen.Instantiate()));
        }

        private void Update()
        {
            Computer.OnUpdate();
            _controller.OnUpdate();
        }
    }
}