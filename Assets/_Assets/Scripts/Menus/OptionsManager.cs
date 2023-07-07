using UnityEngine.UI;

public class OptionsManager : LevelSingleton<OptionsManager>
{
    public SettingsScriptable settings;

    public Slider rumbleSlider;
    public Slider screenShakeSlider;
    public Slider fontSizeSlider;
    public Slider gameplaySpeedSlider;

    private void Awake()
    {
        rumbleSlider.value = settings.rumbleSensitivity;
        screenShakeSlider.value = settings.screenShakeSensitivity;
        fontSizeSlider.value = settings.fontSize;
        gameplaySpeedSlider.value = settings.gameplaySpeed;
    }

    public void RumbleSliderChanged(float value) 
    {
        settings.rumbleSensitivity = value;
    }

    public void ScreenShakeSliderChanged(float value)
    {
        settings.screenShakeSensitivity = value;
    }

    public void FontSizeSliderChanged(float value)
    {
        settings.fontSize = value;
    }

    public void GameplaySpeedSliderChanged(float value)
    {
        settings.gameplaySpeed = value;
        settings.gameplaySpeedChanged.Invoke();
    }
}
