using UnityEngine;
using UnityEngine.SocialPlatforms;

public class DialogueManager : Singleton<DialogueManager>
{
    [HideInInspector]
    public DialogueSection current;
    public bool DialogueActive { get; private set; }

    private bool unpauseNextFrame = false;

    #region Input
    private GameControls gameControls;

    private void Awake()
    {
        gameControls = new GameControls();
    }

    private void OnEnable()
    {
        gameControls.Enable();
    }

    private void OnDisable()
    {
        gameControls.Disable();
    }
    #endregion

    public void StartDialogue(DialogueSection dialogue) 
    {
        current = dialogue;
        current.Reset();

        DialogueActive = true;
        PauseManager.Instance.Pause(PauseManager.PauseType.dialogue);
        if (!PauseManager.Instance.IsPaused(PauseManager.PauseType.menu)) 
        {
            DialoguePanelManager.Instance.Enable();
            Next();
        } 
    }

    private void Update()
    {
        if (unpauseNextFrame) 
        {
            PauseManager.Instance.Unpause(PauseManager.PauseType.dialogue);
            unpauseNextFrame = false;
        }
        if ((!PauseManager.Instance.IsPaused(PauseManager.PauseType.menu)) && DialogueActive && gameControls.Game.Interact.WasPerformedThisFrame() && DialoguePanelManager.Instance.NextReady())
            Next();
    }

    public void Next() 
    {
        var speechData = current.GetNext();
        if (speechData==null) 
        {
            Finish();
            return;
        }
        DialoguePanelManager.Instance.Populate(speechData);
    }

    public void Finish() 
    {
        DialoguePanelManager.Instance.Disable();
        DialogueActive = false;
        current.Reset();
        current = null;
        unpauseNextFrame = true;
    }
}
