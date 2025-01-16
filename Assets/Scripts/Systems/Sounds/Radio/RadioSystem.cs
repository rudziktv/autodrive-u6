using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Systems.AppStorage;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Thread = System.Threading.Thread;

namespace Systems.Sounds.Radio
{
    public class RadioSystem : MonoBehaviour
    {
        private const int BUFFERING_CHECK_TICK = 250;
        private const int BUFFERING_TIMEOUT = 15000;
        
        public static RadioSystem Instance { get; private set; }
        
        private FMOD.System FmodSystem => RuntimeManager.CoreSystem;
        private Sound _radioStream;
        private EventInstance _eventInstance;
        private CancellationTokenSource _tokenSource;
        
        public delegate void StreamBufferingCallback(OPENSTATE openState, uint percentBuffered);

        private void Start()
        {
            Instance = this;
            _tokenSource = new CancellationTokenSource();
        }

        public async void PlayRadio(EventInstance eventInstance, string radioUrl,
            StreamBufferingCallback bufferingCallback = null)
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
            _tokenSource = new CancellationTokenSource();
            StopRadioStream();
            
            var token = _tokenSource.Token;
            _eventInstance = eventInstance;
            
            try
            {
                await PlayRadio(radioUrl, token, bufferingCallback);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        private async Task PlayRadio(string radioUrl, CancellationToken token, StreamBufferingCallback bufferingCallback = null)
        {
            try
            {
                if (!await CreateAndLoadRadioStream(radioUrl, token, bufferingCallback))
                    return;
                
                if (_radioStream.handle == IntPtr.Zero)
                {
                    Debug.LogError("Radio stream handle is invalid.");
                    return;
                }

                _eventInstance.setCallback(EventCallback);
                _eventInstance.start();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async Task<bool> CreateAndLoadRadioStream(string radioUrl, CancellationToken token, StreamBufferingCallback bufferingCallback = null)
        {
            var result = FmodSystem.createStream(radioUrl, MODE.CREATESTREAM | MODE.NONBLOCKING, out _radioStream);

            if (result != RESULT.OK)
            {
                Debug.LogError($"FMOD Error: {result}");
                return false;
            }

            try
            {
                await BufferRadioStream(token, bufferingCallback);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        private async Task BufferRadioStream(CancellationToken token, StreamBufferingCallback bufferingCallback = null)
        {
            OPENSTATE state;
            var timer = 0;
            
            do
            {
                if (token.IsCancellationRequested)
                    return;

                _radioStream.getOpenState(out state, out var percentBuffered, out var _, out var __);
                bufferingCallback?.Invoke(state, percentBuffered);
                try
                {
                    await Task.Delay(BUFFERING_CHECK_TICK, token);
                }
                catch (TaskCanceledException e)
                {
                    return;
                }
                timer += BUFFERING_CHECK_TICK;

                if (timer > BUFFERING_TIMEOUT)
                    Debug.LogWarning($"Could not buffer radio stream within timeout period - {BUFFERING_TIMEOUT}ms.");
                    // throw new Exception($"Could not buffer radio stream within timeout period - {BUFFERING_TIMEOUT}ms.");
                
            } while (state != OPENSTATE.READY);
        }

        [AOT.MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
        private RESULT EventCallback(EVENT_CALLBACK_TYPE type, IntPtr instance, IntPtr parameterPtr)
        {
            Debug.Log($"FMOD EventCallback {type}");
            if (type != EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND) return RESULT.OK;
            var soundInfo = Marshal.PtrToStructure<PROGRAMMER_SOUND_PROPERTIES>(parameterPtr);
            soundInfo.sound = _radioStream.handle;
            Marshal.StructureToPtr(soundInfo, parameterPtr, false);
            return RESULT.OK;
        }
        

        public void StopRadioStream()
        {
            if (_eventInstance.isValid())
                _eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        
        private void OnDestroy()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
            
            if (!_eventInstance.isValid()) return;
            _eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _eventInstance.release();
        }
    }
}