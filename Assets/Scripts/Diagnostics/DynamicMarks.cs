using System.Collections.Generic;
using UnityEngine;

namespace Diagnostics
{
    [ExecuteInEditMode]
    public class DynamicMarks : MonoBehaviour
    {
        [SerializeField]
        private Vector3 markStep = Vector3.zero;

        [SerializeField]
        private bool ignoreFirst = true;
    
        [SerializeField]
        private int amountOfMarks = 9;
    
        [SerializeField]
        private GameObject markPrefab;

        private readonly List<GameObject> _marks = new();

        private void Update()
        {
            for (int j = 0; j < transform.childCount; j++)
            {
                DestroyImmediate(transform.GetChild(j).gameObject);
            }
        
            if (!markPrefab) return;
            foreach (var mark in _marks)
            {
                DestroyImmediate(mark);
            }
        
            _marks.Clear();
        
            var i = ignoreFirst ? 1 : 0;
            for (; i < amountOfMarks; i++)
            {
                var mark = Instantiate(markPrefab, transform);
                mark.transform.position = transform.position + markStep * i;
                _marks.Add(mark);
            }
        }
    }
}