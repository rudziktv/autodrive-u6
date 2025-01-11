using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.Entities.Vehicle.Subentities.LicensePlate
{
    [RequireComponent(typeof(UIDocument))]
    [ExecuteInEditMode]
    public class LicensePlateComponent : MonoBehaviour
    {
        private const int LICENSE_PLATE_WIDTH = 512;
        private const int LICENSE_PLATE_HEIGHT = 96;
        
        [Header("License Plate")]
        [SerializeField] private string prefix;
        [SerializeField] private string suffix;
        [SerializeField] private string countryCode;
        [SerializeField] private Texture2D generatedLicensePlate;
        [SerializeField] private Material sharedMaterial;
        
        [Header("Settings")]
        [SerializeField] private int licensePlateWidth = LICENSE_PLATE_WIDTH;
        [SerializeField] private int licensePlateHeight = LICENSE_PLATE_HEIGHT;
        [SerializeField] private int licensePlatePadding = 2;
        
        private RenderTexture _licenseRenderTexture;
        private UIDocument _ui;

        private void Awake()
        {
        }

        private void Start()
        {
            GenerateLicensePlate();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void GenerateLicensePlate()
        {
            _licenseRenderTexture = new RenderTexture(LICENSE_PLATE_WIDTH, LICENSE_PLATE_HEIGHT, 0);

            _ui = GetComponent<UIDocument>();
            _ui.panelSettings = GetPanelSettings();
            StartCoroutine(Generate());
            // Generate();
        }

        private IEnumerator Generate()
        {
            // _ui.panelSettings = GetPanelSettings();
            _ui.panelSettings.targetTexture = _licenseRenderTexture;
            // setup license plate
            yield return new WaitForEndOfFrame();
            
            var root = _ui.rootVisualElement;
            var plateRoot = root.Q<VisualElement>("license-plate-root");
            
            plateRoot.style.paddingBottom = licensePlatePadding;
            plateRoot.style.paddingTop = licensePlatePadding;
            plateRoot.style.paddingLeft = licensePlatePadding;
            plateRoot.style.paddingRight = licensePlatePadding;
            
            root.Q<Label>("prefix").text = prefix;
            root.Q<Label>("suffix").text = suffix;
            root.Q<Label>("country-code").text = countryCode;
            root.MarkDirtyRepaint();

            yield return new WaitForEndOfFrame();
            
            // var previousTexture = RenderTexture.active;
            RenderTexture.active = _licenseRenderTexture;
            generatedLicensePlate = new Texture2D(LICENSE_PLATE_WIDTH, LICENSE_PLATE_HEIGHT);
            generatedLicensePlate.ReadPixels(new Rect(0, 0, generatedLicensePlate.width, generatedLicensePlate.height), 0, 0);
            generatedLicensePlate.Apply();
            RenderTexture.active = null;
            
            _licenseRenderTexture.Release();
            
            sharedMaterial.mainTexture = generatedLicensePlate;
            
            // DEBUG
            byte[] bytes = generatedLicensePlate.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/LicensePlate.png", bytes);
            UnityEngine.Debug.Log("Texture saved to: " + Application.dataPath + "/LicensePlate.png");
        }

        private PanelSettings GetPanelSettings()
        {
            var set = UnityEngine.Resources.Load<PanelSettings>("GUI/Game/Generative/License Plate/License Plate Settings");
            set.targetTexture = _licenseRenderTexture;
            return set;
        }
    }
}