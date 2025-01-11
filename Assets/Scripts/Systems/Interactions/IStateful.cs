using System;

namespace Systems.Interactions
{
    public interface IStateful<out T>
    {
        public event Action<T> StateChanged;
    }
}