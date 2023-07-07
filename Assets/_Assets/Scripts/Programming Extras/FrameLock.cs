using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameLock : MonoBehaviour
{
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    [SerializeField] private float newTimeScale = 1;

    [ContextMenu("Update Time Scale")]
    private void UpdateTimeScale() 
    {
        Time.timeScale = newTimeScale;
        //Time.fixedDeltaTime *= newTimeScale;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}
