using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanelManager : LevelSingleton<DialoguePanelManager>
{
    public SettingsScriptable settings;

    [SerializeField] private GameObject toggler;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject nextPrompt;
    [HideInInspector] public bool dialoguePanelEnabled = false;

    [Range(0,2)]
    public float continueCooldownTime = 0.8f;
    private Cooldown continueCooldown;

    private Cooldown animateTextCooldown;
    [Range(0, 0.1f)]
    public float timePerCharacter = 0.02f;

    private void Start()
    {
        continueCooldown = new Cooldown(continueCooldownTime, true, EnableNextPrompt);
    }

    public void Enable() 
    {
        toggler.gameObject.SetActive(true);
        dialoguePanelEnabled = true;
    }

    public void Disable() 
    {
        toggler.gameObject.SetActive(false);
        dialoguePanelEnabled = false;
        Reset();
    }

    SpeechData speechData;
    public void Populate(SpeechData speechData) 
    {
        this.speechData = speechData;
        animateTextCooldown = new Cooldown(timePerCharacter*speechData.text.Length, true);
        continueCooldown.Start();
        animateTextCooldown.Start();
        nextPrompt.SetActive(false);
        nameText.text = speechData.speakerName;
        image.sprite = speechData.image;
        text.fontSize = settings.fontSize;
        text.text = "";
    }

    private void Update()
    {
        text.fontSize = settings.fontSize;
        if (PauseManager.Instance.IsPaused(PauseManager.PauseType.menu)) return;

        continueCooldown.Tick(Time.unscaledDeltaTime);
        if (animateTextCooldown != null) 
        {
            animateTextCooldown.Tick(Time.unscaledDeltaTime);

            if (!animateTextCooldown.IsCool() && speechData != null) 
            {
                var t = Mathf.RoundToInt(animateTextCooldown.NormalizedTime() * speechData.text.Length);
                text.text = speechData.text.Substring(0,t);
            }
        }
    }

    private void EnableNextPrompt() 
    {
        nextPrompt.SetActive(true);
    }

    public bool NextReady() 
    {
        return continueCooldown.IsCool();
    }

    public void Reset()
    {
        nameText.text = "";
        text.text = "";
        image.sprite = null;
        speechData = null;
        text.fontSize = settings.fontSize;
    }
}
