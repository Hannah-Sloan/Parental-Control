using UnityEngine;

public class ScreenShaker : Singleton<ScreenShaker>
{
    [Range(0,180)]
    [SerializeField] private float maxRotation = 4f;
    [Range(0,2)]
    [SerializeField] private float maxLateral = .55f;
    [Range(0,100)]
    [SerializeField] private float frequency = 25f;

    public SettingsScriptable settings;

    private float trauma;
    private void Update()
    {
        trauma = Trauma.Instance.GetTraumaTrauma();
        if (trauma < 0) return;
        trauma *= settings.screenShakeSensitivity;

        var t = Time.time * frequency;

        var x = maxLateral * trauma * Mathf.Lerp(-1, 1, Mathf.PerlinNoise(t+324.34f, 0));
        var y = maxLateral * trauma * Mathf.Lerp(-1, 1, Mathf.PerlinNoise(t+842.4f, 0));
        transform.localPosition = new Vector2(x,y);
        transform.localRotation = Quaternion.Euler(0, 0, maxRotation * trauma * Mathf.Lerp(-1, 1, Mathf.PerlinNoise(t + 453.3245f, 0)));
    }
}
