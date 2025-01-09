using Core.Entities.Vehicle.Managers;
using Core.Entities.Vehicle.Modules;
using Core.Helpers;
using UnityEngine;

namespace Core.Entities.Vehicle
{
    public class VehicleController : MonoBehaviour
    {
        [SerializeField] private string vehicleTag;
        [SerializeField] private VehicleConfigManager vehicleConfigManager;
        
        public string VehicleTag => vehicleTag;
        public VehicleConfigManager VehicleConfigManager => vehicleConfigManager;
        
        public ModuleManager ModuleManager { get; private set; }
        public ElectricityManager ElectricityManager { get; private set; }

        public InputActions InputActions { get; private set; }
        public StateController CodingVariables { get; private set; } = new();


        private void Awake()
        {
            InputActions = new ();
            
            ModuleManager = new(this);
            ElectricityManager = new(this);

            ModuleManager.RegisterModule(new ComfortModule(this));
        }

        private void OnEnable()
        {
            InputActions.Enable();
        }

        private void OnDisable()
        {
            InputActions.Disable();
        }

        private void Start()
        {
            ModuleManager.InitializeModules();
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