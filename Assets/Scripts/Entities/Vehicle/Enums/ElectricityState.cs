namespace Entities.Vehicle.Enums
{
    public enum ElectricityState
    {
        Off, // everything dark, driver can turn on only for example radio or hazards, power from battery
        OnlyAccessories, // after turning off the engine, power from battery
        LowPowerMode, // lowers power consumption, during the start of the engine
        Ignition, // everything is powered up, but not from engine, power is from battery
        Engine // everything is powered by engine, through the alternator
    }
}