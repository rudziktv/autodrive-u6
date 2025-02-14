namespace Core.Entities.Vehicle
{
    public interface IVehicleModule
    {
        public void Initialize();
        public void UpdateModule();
        public void FixedUpdateModule();
    }
}