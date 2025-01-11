using System.Collections;
using Core.Patterns.UI;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Systems.Sounds.Music;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

namespace Systems.Devices.Infotainments.RNS510.ViewModels
{
    public class RNS510RadioViewModel : RNS510ViewModel
    {
        private EventInstance _radioEventInstance;

        private readonly string _radioUrl = "https://ic1.smcdn.pl/2330-1.mp3";
        // private readonly string _radioEventPath = "event:/Radio Test";
        
        public RNS510RadioViewModel(UIController<RNS510> ctr, VisualElement view) : base(ctr, view, "Radio") { }

        private bool _fmodStarted;
        
        public override void OnViewBindOrCreate()
        {
            base.OnViewBindOrCreate();
            StartCoroutine(FmodDelay());
            var test = View.Q<Button>("test-btn");
            test.clicked += TestButton;
        }

        private void TestButton()
        {
            Debug.Log("TestButtonOn RNS510");
        }

        private IEnumerator FmodDelay()
        {
            yield return new WaitForSeconds(5f);
            TryFMOD();
        }

        private void TryFMOD()
        {
            _radioEventInstance = RuntimeManager.CreateInstance(Assets.EventReference);
            RadioSystem.Instance.TryToConnect(_radioEventInstance, _radioUrl, ((state, buffered) =>
            {
                var statusLabel = View.Q<Label>("radio-station-status");
                statusLabel.text = state.ToString();
            }));
        }

        public override void OnViewUnbind()
        {
            base.OnViewUnbind();
            var test = View.Q<Button>("test-btn");
            test.clicked -= TestButton;
        }
    }
}