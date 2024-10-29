using UnityEngine;

namespace Resources.Bundles.Cars.Volkswagen_Golf_Mk6.MFA_Plus
{
    public class MFAPlusComputer
    {
        #region Datasets

        public MFAPlusDataset StartupDataset { get; private set; }
        public MFAPlusDataset LongDataset { get; private set; }
        public MFAPlusDataset CurrentDataset => StartupDataset;
        

        #endregion

        private float _tempSensorSensitivity = 0.05f;
        private float _tempOutside = -15f;

        #region Sensors

        private float _temperatureSensor = 8f;

        #endregion

        #region Sensor Outputs
        
        public string TemperatureSensor => GetFloatWithStep(_temperatureSensor, 0.5f).ToString("0.0");

        #endregion

        

        public MFAPlusComputer()
        {
            OnStart();
        }

        private void OnStart()
        {
            OnStartupReset();
            LongDataset = new();
        }
        
        public void OnStartupReset()
        {

            var route = Random.Range(5f, 250f);
            var avgCons = Random.Range(4f, 12f);
            var avgSpeed = Random.Range(25f, 90f);

            
            StartupDataset = new(route, route * avgCons / 100f, route / avgSpeed * 60f * 60f);
        }

        public void OnUpdate()
        {
            StartupDataset.UpdateData(Time.deltaTime, 0f, 0f);

            _temperatureSensor += Mathf.Clamp(
                _tempOutside - _temperatureSensor,
                -_tempSensorSensitivity,
                _tempSensorSensitivity)* Time.deltaTime;
        }

        private float GetFloatWithStep(float value, float step)
        {
            var i = Mathf.RoundToInt(value / step);
            return i * step;
        }
    }
}