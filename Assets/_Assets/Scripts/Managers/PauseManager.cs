using System.Collections.Generic;
using UnityEngine;

public class PauseManager : Singleton<PauseManager>
{
    public enum PauseType 
    {
        menu,
        dialogue,
        cutscene
    }

    private Dictionary<PauseType, bool> pausedLookup = new Dictionary<PauseType, bool>() 
    { 
        {PauseType.menu, false },
        {PauseType.dialogue, false },
        {PauseType.cutscene, false } 
    };

    public delegate void PauseToggledEvent(PauseType pauseType, bool paused);
    public event PauseToggledEvent PauseToggled;

    private float timescale;
    private bool timescaleFilled;

    public bool IsPaused(PauseType pauseType)
    {
        return pausedLookup[pauseType];
    }

    public bool IsPausedAny()
    {
        return IsPaused(PauseType.menu) || IsPaused(PauseType.dialogue) || IsPaused(PauseType.cutscene);
    }

    public void Pause(PauseType pauseType) 
    {
        if (IsPaused(pauseType)) return;
        pausedLookup[pauseType] = true;
        if (!timescaleFilled) 
        {
            timescale = Time.timeScale;
            timescaleFilled = true;
        }
        Time.timeScale = 0;
    }

    public void Unpause(PauseType pauseType) 
    {
        if (!IsPaused(pauseType)) return;
        pausedLookup[pauseType] = false;
        if (timescaleFilled) 
        {
            Time.timeScale = GameplaySpeed.Instance.gameplaySpeed;
            timescaleFilled = false;
        }
    }

    public void TogglePause(PauseType pauseType) 
    {
        if (IsPaused(pauseType)) Unpause(pauseType);
        else Pause(pauseType);
        if (PauseToggled != null) PauseToggled.Invoke(pauseType, !IsPaused(pauseType));
    }
}
