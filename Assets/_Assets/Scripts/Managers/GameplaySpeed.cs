using UnityEngine;

public class GameplaySpeed : Singleton<GameplaySpeed>
{
    public SettingsScriptable settings;

    public float gameplaySpeed { get { return settings.gameplaySpeed; } private set { } }

    private void Awake()
    {
        settings.gameplaySpeedChanged += GameplaySpeedChanged;
        gameplaySpeed = settings.gameplaySpeed;
    }

    private void GameplaySpeedChanged()
    {
        if (PauseManager.Instance.IsPausedAny()) return;

        gameplaySpeed = settings.gameplaySpeed;
    }
}
