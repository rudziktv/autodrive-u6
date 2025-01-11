using Core.Patterns.UI;
using Systems.Devices.TripComputers.MFA_Plus.Models.MFI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Systems.Devices.TripComputers.MFA_Plus.Models
{
    public class MFAPlusDataPageModel : MFAPlusModel
    {
        private VisualElement _main;

        private UIController<MFAPlus> _mfi;

        private MFAPlusModel[] _carouselMfi;
        private int _index = 0;

        private Label _temp;
        
        public MFAPlusDataPageModel(MFAPlusController ctr, VisualElement view) : base(ctr, view, "MFI") { }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();

            _main = View.Q<VisualElement>("main");
            _mfi = new();
            _mfi.Initialize(Context, _main, new MFAPlusModel(_mfi, new VisualElement(), "MFIDefault"));
            
            GenerateCarouselMFI();
            ChangeCarouselMFI();
            
            _temp = View.Q<Label>("temp-value");
            
            ShowHeader();
        }

        protected override void OnViewBind()
        {
            base.OnViewBind();
            Actions.Development.ComputerUp.performed += NextCarouselMFI;
            Actions.Development.ComputerDown.performed += PrevCarouselMFI;
        }

        public override void OnViewUnbind()
        {
            base.OnViewUnbind();
            Actions.Development.ComputerUp.performed -= NextCarouselMFI;
            Actions.Development.ComputerDown.performed -= PrevCarouselMFI;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _mfi.OnUpdate();

            _temp.text = Computer.TemperatureSensor;
            
            if (float.Parse(Computer.TemperatureSensor) <= 4f)
                View.AddToClassList("snowflake-shown");
            else
                View.RemoveFromClassList("snowflake-shown");
        }

        private void NextCarouselMFI(InputAction.CallbackContext ctx)
        {
            _index++;
            ChangeCarouselMFI();
        }

        private void PrevCarouselMFI(InputAction.CallbackContext ctx)
        {
            _index--;
            ChangeCarouselMFI();
        }

        private void ChangeCarouselMFI()
        {
            if (_index >= _carouselMfi.Length)
                _index = 0;
            else if (_index < 0)
                _index = _carouselMfi.Length - 1;
            
            var model = _carouselMfi[_index];
            _mfi.NavigateTo(model);
        }

        private void GenerateCarouselMFI()
        {
            var subpage = Assets.DataSubpage;
            _carouselMfi = new MFAPlusModel[]
            {
                new MFIAvgFuelConsumption(_mfi, subpage.Instantiate()),
                new MFIRoute(_mfi, subpage.Instantiate()),
                new MFIRange(_mfi, subpage.Instantiate()),
                new MFIAvgSpeed(_mfi, subpage.Instantiate()),
                new MFISpeed(_mfi, subpage.Instantiate()),
                new MFIOilTemp(_mfi, subpage.Instantiate()),
                new MFISpeedWarning(_mfi, subpage.Instantiate()),
                new MFITravelTime(_mfi, subpage.Instantiate()),
                new MFIConsumption(_mfi, subpage.Instantiate()),
            };
        }

        public override void ShowHeader()
        {
            base.ShowHeader();
            ShowHeader("MFI");
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _mfi.Dispose();

            foreach (var model in _carouselMfi)
            {
                model.OnDestroy();
            }
        }
    }
}