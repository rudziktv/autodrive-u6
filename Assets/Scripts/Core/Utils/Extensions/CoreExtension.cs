using Systems.Managers;
using UnityEngine;

namespace Core.Utils.Extensions
{
    public static class CoreExtension
    {
        public static GameManager GetGameManager(this object _)
            => GameManager.Instance;
    }
}