namespace Core.Entities.Vehicle.Modules.TransferCase
{
    public interface ITransferCase : IVehicleModule
    {
        public float CurrentRPM { get; }
        
        public void TransferTorque(float torque);
    }
}