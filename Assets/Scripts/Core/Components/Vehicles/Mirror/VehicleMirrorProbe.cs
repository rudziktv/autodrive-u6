using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Core.Components.Vehicles.Mirror
{
    public class VehicleMirrorProbe : MonoBehaviour
    {
        [SerializeField] private AnimationCurve fovFactorCurve;
        [SerializeField] private float fovRange;
        [SerializeField] private PlanarReflectionProbe probe;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            if (probe == null)
                probe = GetComponent<PlanarReflectionProbe>();
            
            if (probe == null)
                Debug.LogError($"Probe component not found on object {name}");
        }

        private void Update()
        {
            probe.settingsRaw.frustum.viewerScale = fovFactorCurve.
                Evaluate(Mathf.Clamp01(_camera.fieldOfView / fovRange));
        }
    }
}