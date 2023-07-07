using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadRumble : MonoBehaviour
{
    private float trauma;

    public SettingsScriptable settings;

    void Update()
    {
        if (PauseManager.Instance.IsPaused(PauseManager.PauseType.menu)) return;
        if (trauma < 0) return;

        if (Gamepad.current == null) return;
        trauma = Trauma.Instance.GetTraumaTrauma();
        trauma *= settings.rumbleSensitivity;
        Gamepad.current.SetMotorSpeeds(trauma, trauma);
    }
}
