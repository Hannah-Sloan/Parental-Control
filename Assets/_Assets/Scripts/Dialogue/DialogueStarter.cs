using UnityEngine;
using UnityEngine.SocialPlatforms;

public class DialogueStarter : MonoBehaviour
{
    public DialogueSection dialogue;
    private GameControls gameControls;
    [SerializeField] private GameObject talkToMe;

    private void Awake()
    {
        gameControls = new GameControls();
        inRange = false;
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

    private bool inRange = false;

    private void Update()
    {
        if(!PauseManager.Instance.IsPaused(PauseManager.PauseType.menu))
            talkToMe.SetActive(inRange);

        if(!PauseManager.Instance.IsPausedAny() && gameControls.Game.Interact.WasPerformedThisFrame() && inRange) 
            DialogueManager.Instance.StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("Player")) inRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) inRange = false;
    }
}
