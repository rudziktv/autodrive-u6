using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Utils
{
    public static class RecursiveChildFinder
    {
        public delegate bool FindChildrenPredicate(GameObject gameObject);
        
        public static GameObject[] FindChildrenWithTagRecursive(GameObject parent, string tag)
        {
            List<GameObject> childrenWithTag = new List<GameObject>();

            foreach (Transform child in parent.transform)
            {
                if (child.CompareTag(tag))
                {
                    childrenWithTag.Add(child.gameObject);
                }

                // Rekurencja dla kolejnych poziomów
                childrenWithTag.AddRange(FindChildrenWithTagRecursive(child.gameObject, tag));
            }

            return childrenWithTag.ToArray();
        }
        
        public static GameObject FindChildWithTagRecursive(GameObject parent, string tag)
            => FindChildWithTagRecursive(parent.transform, tag);
        
        public static GameObject FindChildWithTagRecursive(Transform parent, string tag)
        {
            foreach (Transform child in parent)
            {
                if (child.CompareTag(tag))
                    return child.gameObject;
                if (child.childCount <= 0) continue;
                var recursed = FindChildWithTagRecursive(child, tag);
                if (recursed != null)
                    return recursed;
            }
            return null;
        }
        
        public static GameObject[] FindChildrenRecursive(GameObject parent)
        {
            var childrenWithTag = new List<GameObject>();

            foreach (Transform child in parent.transform)
            {
                childrenWithTag.Add(child.gameObject);
                childrenWithTag.AddRange(FindChildrenRecursive(child.gameObject));
            }

            return childrenWithTag.ToArray();
        }
        
        public static GameObject[] FindChildrenWithNameRecursive(GameObject parent, string name)
        {
            List<GameObject> childrenWithTag = new List<GameObject>();

            foreach (Transform child in parent.transform)
            {
                if (child.name.Equals(name))
                {
                    childrenWithTag.Add(child.gameObject);
                }

                // Rekurencja dla kolejnych poziomów
                childrenWithTag.AddRange(FindChildrenWithTagRecursive(child.gameObject, name));
            }

            return childrenWithTag.ToArray();
        }
        
        public static GameObject[] FindChildrenRecursiveFilter(GameObject parent, FindChildrenPredicate predicate)
        {
            List<GameObject> childrenWithTag = new List<GameObject>();

            foreach (Transform child in parent.transform)
            {
                if (predicate.Invoke(child.gameObject))
                {
                    childrenWithTag.Add(child.gameObject);
                }
                
                childrenWithTag.AddRange(FindChildrenRecursiveFilter(child.gameObject, predicate));
            }

            return childrenWithTag.ToArray();
        }
    }
}