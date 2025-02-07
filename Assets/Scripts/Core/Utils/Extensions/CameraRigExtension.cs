using UnityEngine;

namespace Core.Utils.Extensions
{
    public static class CameraRigExtension
    {
        public static void AttachCameraRig(this GameObject gameObject, bool worldPositionStays = true)
            => AttachCameraRig(gameObject.transform, worldPositionStays);

        public static void AttachCameraRig(this Transform transform, bool worldPositionStays = true)
        {
            var cameraRig = GameObject.FindGameObjectWithTag("CameraRig");
            cameraRig.transform.SetParent(transform, worldPositionStays);
        }
        
        public static void AttachCameraRigSetLocalPos(this GameObject gameObject, Vector3 localPos, bool worldPositionStays = false)
            => AttachCameraRigSetLocalPos(gameObject.transform, localPos, worldPositionStays);

        public static void AttachCameraRigSetLocalPos(this Transform transform, Vector3 localPos, bool worldPositionStays = false)
        {
            var cameraRig = GameObject.FindGameObjectWithTag("CameraRig");
            cameraRig.transform.SetParent(transform, worldPositionStays);
            cameraRig.transform.localPosition = localPos;
        }
    }
}