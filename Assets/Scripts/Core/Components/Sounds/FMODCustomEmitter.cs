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
            if (string.IsNullOrEmpty(bankName))
            {
                var bankContext = GetComponentInParent<FMODBankContext>();
                if (bankContext != null)
                    bankName = bankContext.BankName;
                else
                    throw new ArgumentException("FMODBankContext not found - bankName cannot be null!");
            }
                
            CreateInstance();
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