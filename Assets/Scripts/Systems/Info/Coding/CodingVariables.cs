using System;
using UnityEngine;

namespace Systems.Info.Coding
{
    [Serializable]
    public class CodingVariables
    {
        [field: SerializeField] public string CodingName { get; private set; }
        [SerializeField] private string codingValue;
        
        [field: SerializeField] public CodingVarModeEnum ReadWriteMode { get; private set; }

        public string CodingValue
        {
            get => codingValue;
            set
            {
                if (ReadWriteMode == CodingVarModeEnum.ReadWrite)
                    codingValue = value;
            }
        }
    }
}