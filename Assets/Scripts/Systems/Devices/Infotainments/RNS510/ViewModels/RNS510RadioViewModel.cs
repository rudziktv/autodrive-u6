using System.Collections;
using Core.Patterns.UI;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Devices.Infotainments.RNS510.ViewModels
{
    public class RNS510RadioViewModel : RNS510ViewModel
    {
        private FMOD.System FmodSystem => RuntimeManager.CoreSystem;
        private FMOD.Sound _radioStream;
        private FMOD.Channel _channel;
        private EventInstance _radioEventInstance;

        private readonly string _radioUrl = "https://ic1.smcdn.pl/2330-1.mp3";
        private readonly string _radioEventPath = "event:/Radio Test";
        private ChannelGroup _channelGroup;
        
        public RNS510RadioViewModel(UIController<RNS510> ctr, VisualElement view) : base(ctr, view, "Radio") { }

        private bool _fmodStarted;
        
        public override void OnViewBindOrCreate()
        {
            base.OnViewBindOrCreate();
            // RuntimeManager.CoreSystem.
            // _fmodSystem = new FMOD.System();
            StartCoroutine(FmodDelay());
        }

        private IEnumerator FmodDelay()
        {
            yield return new WaitForSeconds(5f);
            TryFMOD();
        }
        
        private IEnumerator WaitForStreamReady()
        {
            FMOD.OPENSTATE openState;
            uint percentBuffered;
            bool starving;
            bool diskBusy;

            do
            {
                _radioStream.getOpenState(out openState, out percentBuffered, out starving, out diskBusy);
                UnityEngine.Debug.Log($"Loading: {percentBuffered}% - State: {openState}");
                yield return null; // Czekaj jedną klatkę
            } while (openState != FMOD.OPENSTATE.READY);

            var result = FmodSystem.playSound(_radioStream, _channelGroup, false, out _channel);
            
            if (result != FMOD.RESULT.OK)
            {
                UnityEngine.Debug.Log($"FMOD Error: {result}");
                yield break;
            }
            
            _radioEventInstance.start();
            _fmodStarted = true;
        }
        
        

        private void TryFMOD()
        {
            var result = FmodSystem.createStream(_radioUrl, MODE.CREATESTREAM | MODE.NONBLOCKING, out _radioStream);

            if (result != FMOD.RESULT.OK)
            {
                UnityEngine.Debug.Log($"FMOD Error: {result}");
                return;
            }

            _radioEventInstance = RuntimeManager.CreateInstance(Assets.EventReference);
            _radioEventInstance.getChannelGroup(out _channelGroup);
            
            StartCoroutine(WaitForStreamReady());
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _radioEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _radioEventInstance.release();
            _channelGroup.stop();
            _radioStream.release();
            FmodSystem.release();
        }
    }
}