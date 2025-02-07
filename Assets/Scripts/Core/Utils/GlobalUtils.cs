using UnityEngine;

namespace Core.Utils
{
    public static class GlobalUtils
    {
        public static GameObject GetCameraRig()
            => GameObject.FindGameObjectWithTag(GameTags.CAMERA_RIG);

        public static GameObject GetCameraYaw()
            => GameObject.FindGameObjectWithTag(GameTags.CAMERA_YAW);
        
        public static GameObject GetCameraPitch()
            => GameObject.FindGameObjectWithTag(GameTags.CAMERA_PITCH);
    }
}