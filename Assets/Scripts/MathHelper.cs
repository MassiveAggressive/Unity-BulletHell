using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
    public static float OrantilamaYap(float deger, float eskiMin, float eskiMax, float yeniMin, float yeniMax)
    {
        // Eski aralýðý 0 ile 1 aralýðýna dönüþtürün
        float normalizedDeger = Mathf.InverseLerp(eskiMin, eskiMax, deger);

        // Yeni aralýkta deðeri elde edin
        float yeniDeger = Mathf.Lerp(yeniMin, yeniMax, normalizedDeger);

        return yeniDeger;
    }
}
