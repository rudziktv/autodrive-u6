using System;
using System.Collections.Generic;
using Core.Entities.Vehicle.Subentities.Lights;
using Core.Utils;
using UnityEngine;

namespace Core.Entities.Vehicle.Helpers
{
    [Serializable]
    public class DashboardIllumination
    {
        [SerializeField] private IlluminatedItem gauges;
        [SerializeField] private IlluminatedItem needles;
        [SerializeField] private IlluminatedItem buttons;
        [SerializeField] private IlluminatedItem lightSwitch;
        [SerializeField] private Color illuminationColor = Color.white;
        [SerializeField] private string illuminationTag = "DashboardIllumination";
        [SerializeField] private float illuminationIntensity = 10f;
        [SerializeField] private string materialName = "int_labels";

        private IlluminatedItem _illumination;

        public void IlluminateLightSwitch(MonoBehaviour monoBehaviour, float percentage)
        {
            lightSwitch?.ApplyIntensity(monoBehaviour, percentage);
        }
        
        public void FindIlluminationItemsByTag(GameObject root)
        {
            var items = RecursiveChildFinder.FindChildrenWithTagRecursive(root, illuminationTag);
            // var items = GameObject.FindGameObjectsWithTag(illuminationTag);
            
            // Debug.Log($"Found {items.Length} illumination items");
            
            List<EmissionMeshPair> pairs = new();
            MeshRenderer renderer = null;
            
            foreach (var gameObject in items)
            {
                renderer = null;
                gameObject.TryGetComponent(out renderer);
                if (renderer == null)
                    continue;
            
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    var mat = renderer.materials[i];
                    // Debug.Log(mat.name);
                    if (mat.name != $"{materialName} (Instance)")
                        continue;
                    
                    
                    pairs.Add(new(renderer, illuminationIntensity, illuminationColor, i));
                    break;
                }
            }
            
            _illumination = new(pairs.ToArray());

            // _illuminationMaterials = materials.ToArray();
        }

        public void IlluminateButtons(MonoBehaviour monoBehaviour, float percentage)
        {
            buttons.ApplyIntensity(monoBehaviour, percentage);
            _illumination?.ApplyIntensity(monoBehaviour, percentage);
        }

        public void IlluminateGauges(MonoBehaviour monoBehaviour, float percentage)
        {
            gauges.ApplyIntensity(monoBehaviour, percentage);
        }

        public void IlluminateNeedles(MonoBehaviour monoBehaviour,float percentage)
        {
            needles.ApplyIntensity(monoBehaviour, percentage);
        }
    }
}