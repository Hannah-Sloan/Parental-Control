using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PauseMenu : MonoBehaviour
{
    private GameControls gameControls;
    [SerializeField] private GameObject toggler;

    private void Awake()
    {
        gameControls = new GameControls();
        gameControls.Game.Pause.performed += (InputAction.CallbackContext i) => PauseManager.Instance.TogglePause(PauseManager.PauseType.menu);
        PauseManager.Instance.PauseToggled += Pause;
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
        try
        {
            PauseManager.Instance.PauseToggled -= Pause;
        }
        catch (NullReferenceException) { }
    }

    private DepthOfField depthOfField;

    private void Pause(PauseManager.PauseType pauseType, bool isPaused) 
    {
        if (pauseType != PauseManager.PauseType.menu) return;

        toggler.SetActive(!isPaused);
        if(!depthOfField)
            depthOfField = (DepthOfField)(PostProcessingManager.Instance.volume.profile.components.Find(c => c.GetType() == typeof(DepthOfField)));
        depthOfField.active = !isPaused;
    }
}
