using UnityEngine;

namespace UI
{
    public interface IGetController<out T, out C> where T : UIController<C> where C : MonoBehaviour
    {
        public T Controller { get; }
    }
}