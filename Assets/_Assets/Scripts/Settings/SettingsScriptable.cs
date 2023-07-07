using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings")]
public class SettingsScriptable : ScriptableObject
{
    [Range(0, 1)]
    public float rumbleSensitivity = 1;

    [Range(0, 1)]
    public float screenShakeSensitivity = 1;

    [Range(28, 110)]
    public float fontSize = 55;

    [Range(.25f, 1f)]
    public float gameplaySpeed = 1f;

    public Action gameplaySpeedChanged;
}
