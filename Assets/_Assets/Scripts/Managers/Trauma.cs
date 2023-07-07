using UnityEngine;

public class Trauma : Singleton<Trauma>
{
    private float trauma;
    private float traumaTrauma;

    private Cooldown traumaCooldown;
    [Range(0,5)]
    [SerializeField] private float traumaCooldownTime = 0.5f;

    private float traumaTop;

    private void Start()
    {
        trauma = 0;
        traumaTop = 0;
        traumaCooldown = new Cooldown(traumaCooldownTime);
    }

    public void AddTrauma(float toAdd)
    {
        SetTrauma(trauma + toAdd);

        traumaTop = trauma;
        traumaCooldown.Start();
    }

    public float GetTraumaTrauma() 
    {
        return traumaTrauma;
    }

    private void Update()
    {
        if (traumaCooldown.IsCool()) trauma = 0;
        if (trauma < 0) return;

        SetTrauma(Mathf.Lerp(traumaTop, 0, traumaCooldown.NormalizedTime()));
    }

    private void SetTrauma(float newTrauma) 
    {
        trauma = Mathf.Clamp01(newTrauma);
        traumaTrauma = trauma * trauma;
    }
}
