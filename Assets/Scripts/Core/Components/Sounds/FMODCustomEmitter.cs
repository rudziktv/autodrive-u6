using System;
using System.IO;
using System.Linq;
using FMOD.Studio;
using FMODUnity;
using Systems.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Components.Sounds
{
    public class FMODCustomEmitter : MonoBehaviour
    {
        [SerializeField] private string bankName;
        [SerializeField] private EventReference eventReference;
        
        private EventInstance _instance;

        private void Start()
        {
            CreateInstance();
        }
        
        private void Update()
        {
            // if (!_instance.isValid())
            //     CreateInstance();
        
            // var attributes = gameObject.To3DAttributes();
            // _instance.set3DAttributes(attributes);
        }
        
        public void Play()
        {
            if (_instance.isValid())
                _instance.start();
        }
        
        private void CreateInstance()
        {
            var result = GetEventDescriptor().createInstance(out _instance);

            var rb = GetComponentInParent<Rigidbody>();
            
            if (!rb)
                RuntimeManager.AttachInstanceToGameObject(_instance, gameObject);
            else
                RuntimeManager.AttachInstanceToGameObject(_instance, gameObject, rb);
            
            Debug.Log($"Creating instance... {result}");
        }
        
        private EventDescription GetEventDescriptor()
        {
            var bank = FMODBankManager.Instance.LoadBank(bankName);
            
            bank.getEventList(out var events);
            
            foreach (var e in events)
            {
                e.getPath(out var path);
                if (path == eventReference.Path)
                    return e;
            }

            throw new Exception($"Could not find event {eventReference.Path}");
        }
    }
}