using UnityEngine;

namespace Systems.Gamemodes.Base
{
    public interface IGamemode
    {
        public string Tag { get; }
        
        public void EnterMode();
        public void ExitMode();
    }
}