using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PotionItemPickup : MonoBehaviour
{
    private GameControls gameControls;
    [SerializeField] private GameObject pickMeUp;
    [SerializeField] private ItemType itemType;
    [SerializeField] private float harvestTime;
    [SerializeField] private float respawnTime;
    private Cooldown harvestTimeCooldown;
    private Cooldown respawnCooldown;

    [HideInInspector] public bool harvested;

    public enum ItemType 
    {
        none,
        mushroom,
        flower,
        carrot
    }

    private bool inRange = false;

    private void Awake()
    {
        gameControls = new GameControls();
        inRange = false;
        harvestTimeCooldown = new Cooldown(harvestTime);
        respawnCooldown = new Cooldown(respawnTime);
        harvested = false;
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
        if (collision.CompareTag("Player") && !harvested) inRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !harvested) inRange = false;
    }

    private void Update()
    {
        if (!PauseManager.Instance.IsPaused(PauseManager.PauseType.menu))
            pickMeUp.SetActive(inRange);

        if (!PauseManager.Instance.IsPausedAny() && !harvested) 
        {
            if (!gameControls.Game.Interact.IsPressed() || !inRange || PlayerController.Instance.throwing) 
            {
                harvestTimeCooldown.Start();
                PlayerController.Instance.HarvestEnd();
            }
            else 
            {
                PlayerController.Instance.HarvestStart();
            }

            if (gameControls.Game.Interact.IsPressed() && inRange && harvestTimeCooldown.IsCool()) 
            {
                Harvest();
                PlayerController.Instance.HarvestEnd();
            }
        }

        if (!harvested) respawnCooldown.Start();
        if (respawnCooldown.IsCool()) Respawn();
    }

    private void Harvest() 
    {
        harvested = true;
        Debug.Log("Harvested " + itemType.ToString());
        inRange = false;
        pickMeUp.SetActive(false);
        Inventory.Instance.AddItem(itemType);
    }

    private void Respawn() 
    {
        harvested = false;
        respawnCooldown.Start();
    }
}