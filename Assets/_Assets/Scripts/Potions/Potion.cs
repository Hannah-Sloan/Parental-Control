using UnityEngine;

[CreateAssetMenu()]
public class Potion : ScriptableObject
{
    public DamageType resistanceType;
    public DamageType damageType;

    public Potion(DamageType resistanceType, DamageType damageType)
    {
        this.resistanceType = resistanceType;
        this.damageType = damageType;
    }

    public override string ToString()
    {
        return "Resistance Type: " + resistanceType.ToString() + ". Damage Type: " + damageType.ToString();
    }
}

public enum DamageType
{
    fire,
    ice,
    acid,
    secret
}
