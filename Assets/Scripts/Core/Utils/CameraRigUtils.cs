using UnityEngine;

namespace Core.Utils
{
    public static class CameraRigUtils
    {
        public static void DetachCameraRig()
        {
            var rig = GlobalUtils.GetCameraRig();
            rig.transform.parent = null;
        }
    }
}