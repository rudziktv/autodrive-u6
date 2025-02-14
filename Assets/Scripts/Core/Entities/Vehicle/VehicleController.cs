using Core.Entities.Vehicle.Builders;
using Core.Entities.Vehicle.Managers;
using Core.Entities.Vehicle.Modules;
using Core.Entities.Vehicle.Modules.Drivetrain;
using Core.Helpers;
using Systems.Cameras.Vehicle;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Entities.Vehicle
{
    public class VehicleController : MonoBehaviour
    {
        [SerializeField] private string vehicleTag;
        [FormerlySerializedAs("vehicleConfigManager")] [SerializeField] private VehicleConfigManager vehicleConfigs;
        
        public string VehicleTag => vehicleTag;
        public VehicleConfigManager VehicleConfigs => vehicleConfigs;
        public VehicleCameraSystem CameraSystem => VehicleConfigs.ComponentsReferences.CameraSystem;
        
        public ModuleManager ModuleManager { get; private set; }
        public ElectricityManager ElectricityManager { get; private set; }

        public InputActions InputActions { get; private set; }
        public StateController CodingVariables { get; private set; } = new();

        public VehicleInputActions VehicleInput { get; private set; }


        private void Awake()
        {
            VehicleInput = new VehicleInputActions();
            InputActions = new ();
            
            ModuleManager = new(this);
            ElectricityManager = new(this);

            ModuleManager.RegisterModule(new ComfortModule(this));
            ModuleManager.RegisterModule(typeof(DrivetrainModule),
                VehicleConfigs.VehicleData.drivetrain.BuildDrivetrain(this));
        }

        private void OnEnable()
        {
            InputActions.Enable();
            VehicleInput.Enable();
        }

        private void OnDisable()
        {
            InputActions.Disable();
            VehicleInput.Disable();
        }

        private void Start()
        {
            ElectricityManager.Initialize();
            
            ModuleManager.InitializeModules();
            
            VehicleInput.Disable();
            InputActions.Disable();
        }

        private void Update()
        {
            ElectricityManager.Update();
            ModuleManager.UpdateAllModules();
        }

        private void FixedUpdate()
        {
            ModuleManager.FixedUpdateAllModules();
        }

        public bool CompareVehicleTags(string otherTag)
        {
            return vehicleTag.Equals(otherTag);
        }
    }
}