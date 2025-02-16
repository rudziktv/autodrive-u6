using UnityEngine;

namespace Core.Components.Sounds
{
    public class FMODBankContext : MonoBehaviour
    {
        [field: SerializeField] public string BankName { get; private set; }
    }
}