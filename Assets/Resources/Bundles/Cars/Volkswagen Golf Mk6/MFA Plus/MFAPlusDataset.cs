using UnityEngine;

namespace Resources.Bundles.Cars.Volkswagen_Golf_Mk6.MFA_Plus
{
    public class MFAPlusDataset
    {
        public MFAPlusDataset(float route = 0f, float fuelBurnt = 0f, float travelTime = 0f)
        {
            Route = route;
            FuelBurnt = fuelBurnt;
            TravelTime = travelTime;
        }
        
        public float Route { get; private set; }
        public float FuelBurnt { get; private set; }
        public float TravelTime { get; private set; }

        public int TravelTimeHours => Mathf.FloorToInt(TravelTime / 60f / 60f);
        public int TravelTimeMinutes => Mathf.FloorToInt(TravelTime / 60f - TravelTimeHours * 60f);
        public string AvgFuelConsumption => Route > 1 ? (FuelBurnt / Route * 100f).ToString("0.0") : "--.-";
        public string AvgSpeed => Route == 0 ? "---" : Mathf.RoundToInt(Route / (TravelTime / 60f / 60f)).ToString();

        public void UpdateData(float deltaTime, float deltaRoute, float deltaFuel)
        {
            Route += deltaRoute;
            FuelBurnt += deltaFuel;
            TravelTime += deltaTime;
        }
    }
}