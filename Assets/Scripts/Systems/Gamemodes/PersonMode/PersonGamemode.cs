using System;
using Core.Utils;
using Core.Utils.Extensions;
using Systems.Gamemodes.Base;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems.Gamemodes.PersonMode
{
    public class PersonGamemode : IGamemode
    {
        private GameObject _player;
        
        public string Tag => "Person";
        
        public void EnterMode()
        {
            var spawnPoint = GameObject.FindGameObjectWithTag(GameTags.PERSON_SPAWN_POINT);
            if (!spawnPoint)
                throw new NullReferenceException("There is no person spawn point in the scene.");
            
            var prefab = Resources.Load<GameObject>("Prefabs/Person/Person");
            _player = Object.Instantiate(prefab);
            var rigAnchor = _player.FindChildWithTag(GameTags.CAMERA_RIG_ANCHOR);
            rigAnchor.AttachCameraRigSetLocalPos(Vector3.zero);
        }

        public void ExitMode()
        {
            // CameraRigUtils.DetachCameraRig();
            Object.Destroy(_player);
        }
    }
}