using UnityEditor;
using UnityEngine;

namespace Entities.Vehicle.Subentities.LicensePlate
{
    [CustomEditor(typeof(LicensePlateComponent))]
    public class LicensePlateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();
            
            // Wywołaj domyślny rysunek inspektora
            DrawDefaultInspector();

            // Pobierz referencję do komponentu
            LicensePlateComponent myComponent = (LicensePlateComponent)target;

            // Dodaj przycisk
            if (GUILayout.Button("Generate License Plate"))
            {
                // Wywołaj metodę komponentu
                myComponent.GenerateLicensePlate();
            }
        }
    }
}