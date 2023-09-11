using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
    public static float OrantilamaYap(float deger, float eskiMin, float eskiMax, float yeniMin, float yeniMax)
    {
        // Eski aral��� 0 ile 1 aral���na d�n��t�r�n
        float normalizedDeger = Mathf.InverseLerp(eskiMin, eskiMax, deger);

        // Yeni aral�kta de�eri elde edin
        float yeniDeger = Mathf.Lerp(yeniMin, yeniMax, normalizedDeger);

        return yeniDeger;
    }
}
