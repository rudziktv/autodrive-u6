using System.Collections;
using System.Linq;
using Core.Entities.Vehicle.Configs.Comfort;
using Core.Entities.Vehicle.Enums;
using Core.Entities.Vehicle.Modules;
using UnityEngine;

namespace Core.Entities.Vehicle.Submodules.Comfort
{
    public class DuskSensorModule : ElectricalModule
    {
        private static readonly int SourceTexture = UnityEngine.Shader.PropertyToID("source_texture");
        private static readonly int Result = UnityEngine.Shader.PropertyToID("result");
        private static readonly int TextureSize = UnityEngine.Shader.PropertyToID("texture_size");
        private const int SENSOR_RESOLUTION = 32;
        private const float REFRESH_TIME = 0.5f;
        
        private LightsModule LightsModule => GetModule<ComfortModule>().Lights;
        public LightsConfig Config => VehicleConfigs.ComfortConfig.LightsConfig;
        public ComputeShader Shader => Config.DuskSensorShader;
        public DuskSensorModule(VehicleController ctr) : base(ctr) { }
        
        private Coroutine _duskCoroutine;
        private RenderTexture _sensorTexture;
        private ComputeBuffer _resultBuffer;
        private readonly float[] _resultData = new float[SENSOR_RESOLUTION * SENSOR_RESOLUTION];
        
        public float Intensity { get; private set; } = 1f;

        public void StartDuskSensor()
        {
            AllocateResources();
            LightsModule.InteriorLights.DuskSensorUpdate(Intensity);
            _duskCoroutine ??= Controller.StartCoroutine(DuskSensorCoroutine());
        }

        public void StopDuskSensor()
        {
            if (_duskCoroutine == null) return;
            Controller.StopCoroutine(_duskCoroutine);
            _duskCoroutine = null;
            ReleaseResources();
        }

        private IEnumerator DuskSensorCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(REFRESH_TIME);
                var intensity = AvgBrightnessComputeShader();
                LightsModule.InteriorLights.DuskSensorUpdate(intensity);
                Intensity = intensity;
            }
        }

        public float MeasureOnce()
        {
            AllocateResources();
            var avg = AvgBrightnessComputeShader();
            ReleaseResources();
            return avg;
        }

        private void AllocateResources()
        {
            _resultBuffer = new ComputeBuffer(SENSOR_RESOLUTION * SENSOR_RESOLUTION, sizeof(float));
            _sensorTexture ??= new RenderTexture(SENSOR_RESOLUTION, SENSOR_RESOLUTION, 24);
            Config.DuskSensorCamera.targetTexture = _sensorTexture;
        }

        private void ReleaseResources()
        {
            _resultBuffer?.Release();
            _sensorTexture?.Release();
            
            _resultBuffer = null;
            _sensorTexture = null;
        }
        
        private float AvgBrightnessComputeShader()
        {
            int kernel = Shader.FindKernel("CSMain");
            
            Shader.SetTexture(kernel, SourceTexture, _sensorTexture);
            Shader.SetBuffer(kernel, Result, _resultBuffer);
            Shader.SetInt(TextureSize, SENSOR_RESOLUTION);
            
            int threadGroupsX = Mathf.CeilToInt(SENSOR_RESOLUTION / 16f);
            int threadGroupsY = Mathf.CeilToInt(SENSOR_RESOLUTION / 16f);
            Shader.Dispatch(kernel, threadGroupsX, threadGroupsY, 1);
            _resultBuffer.GetData(_resultData);
            
            var avg = _resultData.Average() * 4;
            return avg;
        }

        public RuntimeLightsState DuskSense()
        {
            LightsModule.InteriorLights.DuskSensorUpdate(Intensity);
            return Intensity < Config.DuskSensorThreshold ? RuntimeLightsState.LowBeams : RuntimeLightsState.Daylights;
        }

        public override void UpdateModule()
        {
            base.UpdateModule();
            // Debug.Log($"Dusk Sensor: {Intensity}");
            LightsModule.OnDuskDetected(DuskSense());
        }
    }
}