using System;
using Core.Components.Vehicles;
using Core.Entities.Vehicle.Configs.Drivetrain;
using Core.Entities.Vehicle.Data.Drivetrain.Differential;

namespace Core.Entities.Vehicle.Modules.Differential
{
    public class OpenDifferential : IDifferential
    {
        public AxleConfig Axle { get; }
        public OpenDifferentialData Data { get; }
        public VehicleController Controller { get; }
        
        public float CurrentRPM { get; set; }

        public TireComponent LeftTire { get; }
        public TireComponent RightTire { get; }

        private float _transferTorque;

        public OpenDifferential(OpenDifferentialData data, AxleConfig axle, VehicleController ctr)
        {
            Data = data;
            Axle = axle;
            Controller = ctr;
            
            if (Axle.Tires.Length != 2)
                throw new Exception("Open Differential Axle must have two tires.");
            
            LeftTire = Axle.Tires[0];
            RightTire = Axle.Tires[1];
        }
        
        public void Initialize()
        {
            // throw new System.NotImplementedException();
        }

        public void UpdateModule()
        {
            // throw new System.NotImplementedException();
        }

        public void FixedUpdateModule()
        {
            // 50%/50% fixed wheel torque
            var tireTorque = _transferTorque * 0.5f;
            LeftTire.MotorTorque = tireTorque;
            RightTire.MotorTorque = tireTorque;
            
            CurrentRPM = LeftTire.RPM + RightTire.RPM / 2f;
        }

        public void TransferTorque(float torque)
        {
            _transferTorque = torque;
        }
    }
}