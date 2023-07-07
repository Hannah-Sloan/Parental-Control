using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reseter : Singleton<Reseter>
{
    public static Action Reset;

    private GameControls gameControls;

    private float resetTimer;
    [SerializeField] private float resetTimerTime;

    private void Awake()
    {
        gameControls = new GameControls();
    }

    private void Start()
    {
        resetTimer = resetTimerTime;
    }

    private void OnEnable()
    {
        gameControls.Enable();
    }

    private void OnDisable()
    {
        gameControls.Disable();
    }

    void FixedUpdate()
    {
        if (gameControls.Game.Reset.ReadValue<float>() <= 0.5f)
            resetTimer = resetTimerTime;
        else
            resetTimer -= Time.deltaTime;

        if (resetTimer < 0)
        {
            ResetMethod();
        }
    }

    public void ResetMethod() 
    {
        resetTimer = resetTimerTime;
        Reset?.Invoke();
        SceneManager.LoadScene(1);
    }
}
