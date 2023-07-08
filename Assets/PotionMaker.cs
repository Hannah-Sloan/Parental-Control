using System.Collections.Generic;
using UnityEngine;

public class PotionMaker : MonoBehaviour
{
    private GameControls gameControls;
    [SerializeField] private GameObject useMe;
    [SerializeField] private float potionCookTime;
    private Cooldown potionCookCooldown;

    public enum DamageType
    {
        fire,
        ice,
        acid,
        secret
    }

    public static Potion fireFire = new Potion(DamageType.fire, DamageType.fire);
    public static Potion fireIce = new Potion(DamageType.fire, DamageType.ice);
    public static Potion fireAcid = new Potion(DamageType.fire, DamageType.acid);
    public static Potion iceFire = new Potion(DamageType.ice, DamageType.fire);
    public static Potion iceIce = new Potion(DamageType.ice, DamageType.ice);
    public static Potion iceAcid = new Potion(DamageType.ice, DamageType.acid);
    public static Potion acidFire = new Potion(DamageType.acid, DamageType.fire);
    public static Potion acidIce = new Potion(DamageType.acid, DamageType.ice);
    public static Potion acidAcid = new Potion(DamageType.acid, DamageType.acid);
    public static Potion secret = new Potion(DamageType.secret, DamageType.secret);

    public static Potion[] potions = new Potion[]
    {
        fireFire,
        fireIce,
        fireAcid,
        iceFire,
        iceIce,
        iceAcid,
        acidFire,
        acidIce,
        acidAcid,
        secret
    };

    //Shroom, Flower, Carrot
    public static Dictionary<(int,int,int), Potion> recipeLookup = new Dictionary<(int,int,int), Potion>()
    {
        // all carrot = fire potion
        {(0,0,3),fireFire},
        // all flower = ice potion
        {(0,3,0),iceIce},
        // all mushroom = acid potion
        {(3,0,0),acidAcid},
        
        {(2,1,0), iceAcid},
        {(1,2,0), acidIce},
        {(2,0,1), fireAcid},
        {(1,0,2), acidFire},
        {(0,2,1), fireIce},
        {(0,1,2), iceFire},

        // variety = secret potion
        {(1,1,1),secret},
    };
    
    private bool inRange = false;
    private void Awake()
    {
        gameControls = new GameControls();
        inRange = false;
        potionCookCooldown = new Cooldown(potionCookTime);
        potionCookCooldown.SetCool();
        potionCookCooldown.Cool += FinishPotion;
    }

    private void OnEnable()
    {
        gameControls.Enable();
    }

    private void OnDisable()
    {
        gameControls.Disable();
    }

    private void OnDestroy()
    {
        inRange = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) inRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) inRange = false;
    }

    private void Update()
    {
        if (!PauseManager.Instance.IsPaused(PauseManager.PauseType.menu))
            useMe.SetActive(inRange && potionCookCooldown.IsCool());
        
        if (!PauseManager.Instance.IsPausedAny())
        {
            if (gameControls.Game.Interact.WasPerformedThisFrame() && potionCookCooldown.IsCool() && inRange) 
            {
                MakeAPotion();
            }
        }
    }

    public Potion potion;

    public void MakeAPotion() 
    {
        if (
            Inventory.Instance.item1 == PotionItemPickup.ItemType.none ||
            Inventory.Instance.item2 == PotionItemPickup.ItemType.none ||
            Inventory.Instance.item3 == PotionItemPickup.ItemType.none
        ) 
        {
            Debug.Log("More Items Needed!");
            return;
        }

        // Consume Inventory
        int m = 0;
        int f = 0;
        int c = 0;

        var inv = new PotionItemPickup.ItemType[] { Inventory.Instance.item1, Inventory.Instance.item2, Inventory.Instance.item3 };

        foreach (var item in inv)
        {
            switch (item)
            {
                case PotionItemPickup.ItemType.mushroom: m++; break;
                case PotionItemPickup.ItemType.flower: f++; break;
                case PotionItemPickup.ItemType.carrot: c++; break;
            }
        }

        Inventory.Instance.Clear();

        potion = recipeLookup[(m,f,c)];

        potionCookCooldown.Start();
    }

    public void FinishPotion() 
    {
        Debug.Log("POTION! " + potion);
        potion = null;
    }
}

public class Potion 
{
    public PotionMaker.DamageType resistanceType;
    public PotionMaker.DamageType damageType;

    public Potion(PotionMaker.DamageType resistanceType, PotionMaker.DamageType damageType)
    {
        this.resistanceType = resistanceType;
        this.damageType = damageType;
    }

    public override string ToString()
    {
        return "Resistance Type: " + resistanceType.ToString() + ". Damage Type: " + damageType.ToString();
    }
}
