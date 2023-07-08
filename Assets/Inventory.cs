using UnityEngine;

public class Inventory : LevelSingleton<Inventory>
{
    public PotionItemPickup.ItemType item1;
    public PotionItemPickup.ItemType item2;
    public PotionItemPickup.ItemType item3;

    public void Clear() 
    {
        item1 = PotionItemPickup.ItemType.none;
        item2 = PotionItemPickup.ItemType.none;
        item3 = PotionItemPickup.ItemType.none;
    }

    public void AddItem(PotionItemPickup.ItemType item) 
    {
        if (item1 == PotionItemPickup.ItemType.none) item1 = item;
        else if (item2 == PotionItemPickup.ItemType.none) item2 = item;
        else if (item3 == PotionItemPickup.ItemType.none) item3 = item;
        else
            Debug.Log("Inventory Full!");
    }
}
