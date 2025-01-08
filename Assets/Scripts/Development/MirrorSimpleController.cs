using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Development
{
    public class MirrorSimpleController : MonoBehaviour
    {
        [SerializeField] private PlanarReflectionProbe reflectionProbe;
        [SerializeField] private Transform target;
        [SerializeField] private float yaw;
        [SerializeField] private float pitch;

        [SerializeField] private bool frameLimit = true;
        [Range(15, 165)] [SerializeField] private int frameRate = 60;
        

        private Vector3 _baseRotation;

        private void Start()
        {
           _baseRotation = target.localEulerAngles;
           if (frameLimit)
           {
               reflectionProbe.realtimeMode = ProbeSettings.RealtimeMode.OnDemand;
               StartCoroutine(Reflect());
               return;
           }

           reflectionProbe.realtimeMode = ProbeSettings.RealtimeMode.EveryFrame;
        }

        private void Update()
        {
            var rot = _baseRotation;
            rot.z += yaw;
            rot.x += pitch;
            target.localEulerAngles = rot;
        }

        private IEnumerator Reflect()
        {
            while (true)
            {
                reflectionProbe.RequestRenderNextUpdate();
                yield return new WaitForSeconds(1f / frameRate);
            }
        }
    }
}