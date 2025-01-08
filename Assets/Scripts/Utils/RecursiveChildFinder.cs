using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
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