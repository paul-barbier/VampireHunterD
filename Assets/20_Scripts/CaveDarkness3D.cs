using UnityEngine;

public class CaveDarkness3D : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Light directionalLight;

    [Header("Depth Settings")]
    public float startDarkY = 5f;
    public float fullDarkY = -20f;

    [Header("Light Settings")]
    public float minLightIntensity = 0.1f;
    public float maxLightIntensity = 1f;

    [Header("Fog Settings")]
    public float minFogDensity = 0.002f;
    public float maxFogDensity = 0.1f;
    public Color fogColorBright = Color.gray;
    public Color fogColorDark = Color.black;

    bool isLightRestored = false;

    void Start()
    {
        // Assure que le fog est activé
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;
    }

    void Update()
    {
        if (isLightRestored) return;

        float y = player.position.y;

        if (y > startDarkY) return;

        float t = Mathf.InverseLerp(startDarkY, fullDarkY, y);

        // LUMIÈRE
        directionalLight.intensity = Mathf.Lerp(maxLightIntensity, minLightIntensity, t);

        // FOG
        RenderSettings.fogDensity = Mathf.Lerp(minFogDensity, maxFogDensity, t);
        RenderSettings.fogColor = Color.Lerp(fogColorBright, fogColorDark, t);
    }

    public void RestoreLight()
    {
        isLightRestored = true;
        StartCoroutine(FadeLightIn());
    }

    System.Collections.IEnumerator FadeLightIn()
    {
        float t = 0f;

        float startIntensity = directionalLight.intensity;
        float startFogDensity = RenderSettings.fogDensity;
        Color startFogColor = RenderSettings.fogColor;

        while (t < 1f)
        {
            t += Time.deltaTime * 0.5f;

            directionalLight.intensity = Mathf.Lerp(startIntensity, maxLightIntensity, t);
            RenderSettings.fogDensity = Mathf.Lerp(startFogDensity, minFogDensity, t);
            RenderSettings.fogColor = Color.Lerp(startFogColor, fogColorBright, t);

            yield return null;
        }
    }
}
