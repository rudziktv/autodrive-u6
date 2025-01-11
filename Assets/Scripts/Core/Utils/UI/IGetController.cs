using Core.Patterns.UI;
using UnityEngine;

namespace Core.Utils.UI
{
    public interface IGetController<out T, out C> where T : UIController<C> where C : MonoBehaviour
    {
        public T Controller { get; }
    }
}