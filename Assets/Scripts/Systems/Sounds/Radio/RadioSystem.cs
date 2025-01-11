using System.Collections;
using System.Collections.Generic;
using System.IO;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Systems.AppStorage;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Systems.Sounds.Radio
{
    public class RadioSystem : MonoBehaviour
    {
        public static RadioSystem Instance { get; private set; }
        
        private FMOD.System FmodSystem => RuntimeManager.CoreSystem;
        private FMOD.Sound _radioStream;
        private FMOD.Channel _channel;
        private FMOD.ChannelGroup _group;
        private EventInstance _eventInstance;
        
        public delegate void StreamLoadingCallback(OPENSTATE openState, uint percentBuffered);

        private void Start()
        {
            Instance = this;
        }

        public void TryToConnect(EventInstance eventInstance, string radioUrl, StreamLoadingCallback streamLoadingCallback = null)
        {
            StopRadioStream();
            
            _eventInstance = eventInstance;
            var result = FmodSystem.createStream(radioUrl, MODE.CREATESTREAM | MODE.NONBLOCKING, out _radioStream);

            if (result != RESULT.OK)
            {
                Debug.Log($"FMOD Error: {result}");
                return;
            }
            
            eventInstance.getChannelGroup(out _group);

            StartCoroutine(LoadRadioStream(streamLoadingCallback));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator LoadRadioStream(StreamLoadingCallback streamLoadingCallback = null)
        {
            OPENSTATE openState;
            uint percentBuffered;
            bool starving;
            bool diskBusy;

            do
            {
                _radioStream.getOpenState(out openState, out percentBuffered, out starving, out diskBusy);
                Debug.Log($"Loading: {percentBuffered}% - State: {openState}");
                streamLoadingCallback?.Invoke(openState, percentBuffered);
                yield return null;
            } while (openState != OPENSTATE.READY);

            var result = FmodSystem.playSound(_radioStream, _group, false, out _channel);
            
            if (result != RESULT.OK)
            {
                Debug.Log($"FMOD Error: {result}");
                yield break;
            }
            
            _eventInstance.start();
        }

        public void StopRadioStream()
        {
            if (_eventInstance.isValid())
            {
                _eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                // _eventInstance.release();
            }

            _group.stop();
            _radioStream.release();
        }
        
        private void OnDestroy()
        {
            if (_eventInstance.isValid())
            {
                _eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                _eventInstance.release();
            }
            _group.stop();
            _radioStream.release();
        }
    }
}