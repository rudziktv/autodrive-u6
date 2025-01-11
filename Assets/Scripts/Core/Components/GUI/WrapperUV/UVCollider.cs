using System;
using UnityEngine;

namespace Core.Components.GUI.WrapperUV
{
    [RequireComponent(typeof(MeshCollider))]
    public class UVCollider : MonoBehaviour
    {
        [SerializeField] private Vector2 uvSize;
        
        private MeshCollider _collider;

        private void Start()
        {
            _collider = GetComponent<MeshCollider>();
            Debug.Log(_collider.bounds.size);
        }

        // TODO its just approximation now!
        public Vector2 GetUV(Vector3 pointWorldSpace)
        {
            var uv = Vector2.zero;
            var localHitPoint = transform.InverseTransformPoint(pointWorldSpace);

            var width = uvSize.x;
            var height = uvSize.y;
            
            
            uv.x = Mathf.Clamp01((-localHitPoint.x) / width * transform.localScale.x + 0.5f);
            uv.y = Mathf.Clamp01((localHitPoint.z) / height * transform.localScale.z + 0.5f);
            
            // Debug.Log($"UV: {uv}");
            return uv;
        }
    }
}