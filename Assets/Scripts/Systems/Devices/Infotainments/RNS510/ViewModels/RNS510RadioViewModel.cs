using System;
using System.Collections;
using System.Collections.Generic;
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

        private Label _radioStationIndicator;
        private Label _radioStationInfo;
        private Label _radioStationDetails;
        // private readonly string _radioEventPath = "event:/Radio Test";
        
        public RNS510RadioViewModel(UIController<RNS510> ctr, VisualElement view) : base(ctr, view, "Radio") { }

        private bool _fmodStarted;

        private Action _clearStationList;
        
        public override void OnViewBindOrCreate()
        {
            base.OnViewBindOrCreate();

            _radioStationIndicator = View.Q<Label>("radio-station-indicator");
            _radioStationInfo = View.Q<Label>("radio-station-info");
            _radioStationDetails = View.Q<Label>("radio-station-details");

            UpdateStationList();
        }

        private void UpdateStationList()
        {
            var stationsView = View.Q<VisualElement>("radio-stations");
            stationsView.Clear();
            var onlineStations = RadioStationsManager.userStations;

            var actions = new List<Action>();

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

                var i1 = i + 1;
                btn.clicked += Click;
                stationsView.Add(btn);
                actions.Add(() =>
                {
                    btn.clicked -= Click;
                });


                void Click()
                {
                    _radioStationIndicator.text = $"Online: {i1}";
                    ConnectWithOnlineStation(station);
                }
            }

            _clearStationList = () =>
            {
                foreach (var a in actions)
                {
                    a?.Invoke();
                }
            };
        }

        private void ConnectWithOnlineStation(RadioStation station)
        {
            _radioEventInstance = RuntimeManager.CreateInstance(Assets.EventReference);
            RuntimeManager.AttachInstanceToGameObject(_radioEventInstance, Assets.SpeakerTransform, 
                Context.GetComponentInParent<Rigidbody>());
            RadioSystem.Instance.PlayRadio(_radioEventInstance, station.url, ((state, buffered) =>
            {
                var statusLabel = View.Q<Label>("radio-station-status");
                statusLabel.text = state.ToString();
            }));
            _radioStationInfo.text = station.name;
            _radioStationDetails.text = station.details;
        }

        public override void OnViewUnbind()
        {
            base.OnViewUnbind();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _clearStationList?.Invoke();
        }
    }
}