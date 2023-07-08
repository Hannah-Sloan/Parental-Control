using System;
using System.Collections.Generic;
using UnityEngine;

public class PotionMaker : MonoBehaviour
{
    private GameControls gameControls;
    [SerializeField] private GameObject useMe;
    [SerializeField] private float potionCookTime;
    [SerializeField] private RecipeBook recipes;
    private Cooldown potionCookCooldown;

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

    [HideInInspector] public Potion potion;

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

        potion = recipes.recipes.Find(r => r.recipe == new Vector3(m,f,c)).potion;

        potionCookCooldown.Start();
    }

    public void FinishPotion() 
    {
        Debug.Log("POTION! " + potion);
        Inventory.Instance.potion = potion;
        potion = null;
    }
}


