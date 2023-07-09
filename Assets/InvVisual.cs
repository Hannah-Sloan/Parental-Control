using UnityEngine;

public class InvVisual : MonoBehaviour
{
    public GameObject slot1Mushroom;
    public GameObject slot1Flower;
    public GameObject slot1Carrot;

    [Space(20)]
    public GameObject slot2Mushroom;
    public GameObject slot2Flower;
    public GameObject slot2Carrot;

    [Space(20)]
    public GameObject slot3Mushroom;
    public GameObject slot3Flower;
    public GameObject slot3Carrot;

    [Space(20)]
    public GameObject potion;

    private void Update()
    {
        potion.SetActive(Inventory.Instance.potion != null);

        switch (Inventory.Instance.item1)
        {
            case PotionItemPickup.ItemType.none:
                slot1Mushroom.SetActive(false);
                slot1Flower.SetActive(false);
                slot1Carrot.SetActive(false);
                break;
            case PotionItemPickup.ItemType.mushroom:
                slot1Mushroom.SetActive(true);
                slot1Flower.SetActive(false);
                slot1Carrot.SetActive(false);
                break;
            case PotionItemPickup.ItemType.flower:
                slot1Mushroom.SetActive(false);
                slot1Flower.SetActive(true);
                slot1Carrot.SetActive(false);
                break;
            case PotionItemPickup.ItemType.carrot:
                slot1Mushroom.SetActive(false);
                slot1Flower.SetActive(false);
                slot1Carrot.SetActive(true);
                break;
        }

        switch (Inventory.Instance.item2)
        {
            case PotionItemPickup.ItemType.none:
                slot2Mushroom.SetActive(false);
                slot2Flower.SetActive(false);
                slot2Carrot.SetActive(false);
                break;
            case PotionItemPickup.ItemType.mushroom:
                slot2Mushroom.SetActive(true);
                slot2Flower.SetActive(false);
                slot2Carrot.SetActive(false);
                break;
            case PotionItemPickup.ItemType.flower:
                slot2Mushroom.SetActive(false);
                slot2Flower.SetActive(true);
                slot2Carrot.SetActive(false);
                break;
            case PotionItemPickup.ItemType.carrot:
                slot2Mushroom.SetActive(false);
                slot2Flower.SetActive(false);
                slot2Carrot.SetActive(true);
                break;
        }

        switch (Inventory.Instance.item3)
        {
            case PotionItemPickup.ItemType.none:
                slot3Mushroom.SetActive(false);
                slot3Flower.SetActive(false);
                slot3Carrot.SetActive(false);
                break;
            case PotionItemPickup.ItemType.mushroom:
                slot3Mushroom.SetActive(true);
                slot3Flower.SetActive(false);
                slot3Carrot.SetActive(false);
                break;
            case PotionItemPickup.ItemType.flower:
                slot3Mushroom.SetActive(false);
                slot3Flower.SetActive(true);
                slot3Carrot.SetActive(false);
                break;
            case PotionItemPickup.ItemType.carrot:
                slot3Mushroom.SetActive(false);
                slot3Flower.SetActive(false);
                slot3Carrot.SetActive(true);
                break;
        }
    }
}
