namespace Core.Entities.Vehicle
{
    public interface IModuleConfig<out T>
    {
        public T Config { get; }
    }
}