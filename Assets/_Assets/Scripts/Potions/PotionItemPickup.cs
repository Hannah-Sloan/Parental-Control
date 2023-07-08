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

    [SerializeField] private Color disabledColor;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private AnimationCurve fadeInCurve;

    [HideInInspector] public bool harvested;

    public enum ItemType 
    {
        none,
        mushroom,
        flower,
        carrot
    }

    private bool inRange = false;
    public bool harvesting;

    private void Awake()
    {
        gameControls = new GameControls();
        inRange = false;
        harvestTimeCooldown = new Cooldown(harvestTime);
        respawnCooldown = new Cooldown(respawnTime);
        harvested = false;
        harvesting = false;
    }

    private void Start()
    {
        IngredientManager.Instance.Register(this);
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
                harvesting = false;
            }
            else 
            {
                harvesting = true;
            }

            if (gameControls.Game.Interact.IsPressed() && inRange && harvestTimeCooldown.IsCool()) 
            {
                Harvest();
                harvesting = false;
            }
        }

        if (!harvested) respawnCooldown.Start();
        else 
        {
            spriteRenderer.color = Color.Lerp(disabledColor, Color.white, respawnCooldown.NormalizedTime()); 
        }
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
        spriteRenderer.color = Color.white;
    }
}