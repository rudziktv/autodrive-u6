using System;

namespace Interactions
{
    public interface IStateful<out T>
    {
        public event Action<T> OnStateChanged;
    }
}