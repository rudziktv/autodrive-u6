using Systems.Interactions;

namespace Core.Entities.Vehicle.Interactions
{
    public class DriverSeatInteractable : VehicleInteractable
    {
        public override void SimpleInteract()
        {
            base.SimpleInteract();
            Vehicle.CameraSystem.EnterVehicleAsDriver();
        }
    }
}