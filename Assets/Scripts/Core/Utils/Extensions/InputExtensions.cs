using UnityEngine.InputSystem;

namespace Core.Utils.Extensions
{
    public static class InputExtensions
    {
        public static InputAction GetInputAction(this object obj, string actionName)
            => InputSystem.actions.FindAction(actionName);
    }
}