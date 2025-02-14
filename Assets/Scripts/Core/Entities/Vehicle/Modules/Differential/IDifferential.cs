namespace Core.Entities.Vehicle.Modules.Differential
{
    public interface IDifferential : IVehicleModule
    {
        public float CurrentRPM { get; }
        
        public void TransferTorque(float torque);
    }
}