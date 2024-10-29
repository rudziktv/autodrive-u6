using System.Collections;
using Resources.Bundles.Cars.Volkswagen_Golf_Mk6.MFA_Plus.Models.MFI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Resources.Bundles.Cars.Volkswagen_Golf_Mk6.MFA_Plus.Models
{
    public class MFAPlusDataPageModel : MFAPlusModel
    {
        private VisualElement _main;
        
        private VisualElement _topSect;
        private Label _header;

        private MFAPlusController _mfi;

        private MFAPlusModel[] _carouselMfi;
        private int _index = 0;

        private Label _temp;
        
        public MFAPlusDataPageModel(MFAPlusController ctr, VisualElement view) : base(ctr, view, "MFI") { }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();

            _main = View.Q<VisualElement>("main");
            _mfi = new(Context, _main);
            GenerateCarouselMFI();
            // _mfi.NavigateTo(new MFIAvgFuelConsumption(_mfi, Context.Assets.DataSubpage.Instantiate()));
            ChangeCarouselMFI();


            _topSect = View.Q<VisualElement>("top-sect");
            _header = View.Q<Label>("header");
            _temp = View.Q<Label>("temp-value");

            // assign MFI header on top of screen
            _header.text = "MFI";
            _topSect.AddToClassList("header-shown");
            StartCoroutine(ShowHeader());
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
            var subpage = Context.Assets.DataSubpage;
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

        private IEnumerator ShowHeader()
        {
            yield return new WaitForSeconds(5);
            _topSect.RemoveFromClassList("header-shown");
        }
    }
}