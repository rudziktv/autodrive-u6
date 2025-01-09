using UnityEngine;

namespace Core.Utils
{
    public static class TextureUtils
    {
        public static Texture2D BlendTextures(Texture2D tex1, Texture2D tex2, float blend)
        {
            // Sprawdź, czy tekstury mają te same wymiary
            if (tex1.width != tex2.width || tex1.height != tex2.height)
            {
                // Debug.LogError("Tekstury muszą mieć te same wymiary!");
                return null;
            }

            // Tworzymy nową teksturę
            var result = new Texture2D(tex1.width, tex1.height, TextureFormat.RGBA32, false);

            // Pobierz piksele z obu tekstur
            var pixels1 = tex1.GetPixels();
            var pixels2 = tex2.GetPixels();

            // Tworzymy tablicę pikseli dla wyniku
            var blendedPixels = new Color[pixels1.Length];

            // Mieszamy piksele
            for (int i = 0; i < pixels1.Length; i++)
            {
                blendedPixels[i] = Color.Lerp(pixels1[i], pixels2[i], blend); // Interpolacja między kolorami
            }

            // Ustaw piksele w nowej teksturze
            result.SetPixels(blendedPixels);
            result.Apply(); // Zastosowanie zmian w teksturze

            return result;
        }
    }
}