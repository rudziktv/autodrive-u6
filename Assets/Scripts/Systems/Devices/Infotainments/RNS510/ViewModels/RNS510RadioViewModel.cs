using System.Collections;
using Core.Patterns.UI;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Systems.Sounds.Radio;
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

            UpdateStationList();
        }

        private void UpdateStationList()
        {
            var stationsView = View.Q<VisualElement>("radio-stations");
            stationsView.Clear();
            var onlineStations = RadioStationsManager.userStations;

            for (int i = 0; i < 6; i++)
            {
                Button btn;
                
                if (i >= onlineStations.Length)
                {
                    btn = new Button
                    {
                        text = $"{i + 1}. Empty"
                    };
                    btn.SetEnabled(false);
                    stationsView.Add(btn);
                    continue;
                }

                var station = onlineStations[i];
                btn = new Button
                {
                    text = 1 + i + ". " + station.name
                };
                btn.clicked += () =>
                {
                    ConnectWithOnlineStation(station.url);
                };
                stationsView.Add(btn);
            }
        }

        private IEnumerator FmodDelay()
        {
            yield return new WaitForSeconds(5f);
            TryFMOD();
        }

        private void ConnectWithOnlineStation(string url)
        {
            _radioEventInstance = RuntimeManager.CreateInstance(Assets.EventReference);
            RadioSystem.Instance.TryToConnect(_radioEventInstance, url, ((state, buffered) =>
            {
                var statusLabel = View.Q<Label>("radio-station-status");
                statusLabel.text = state.ToString();
            }));
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
        }
    }
}