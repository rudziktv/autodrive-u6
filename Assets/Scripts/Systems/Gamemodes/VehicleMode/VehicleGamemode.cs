using Core.Entities.Vehicle;
using Core.Utils.Extensions;
using Systems.Gamemodes.Base;
using UnityEngine;

namespace Systems.Gamemodes.VehicleMode
{
    public class VehicleGamemode : IGamemode
    {
        public string Tag => "Vehicle";

        private readonly VehicleController _vehicle;

        public VehicleGamemode(VehicleController vehicle)
        {
            this._vehicle = vehicle;
        }
        
        public void EnterMode()
        {
            _vehicle.InputActions.Enable();
            _vehicle.VehicleInput.Enable();
            // _vehicle.CameraSystem.Anchor.AttachCameraRigSetLocalPos(Vector3.zero);
        }

        public void ExitMode()
        {
            _vehicle.InputActions.Disable();
            _vehicle.VehicleInput.Disable();
        }
    }
}