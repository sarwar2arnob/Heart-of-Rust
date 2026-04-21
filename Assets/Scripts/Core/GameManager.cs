// Core/GameManager.cs

using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Singleton;

public enum GameState { Boot, MainMenu, Playing, Paused, GameOver } // 👈 added GameOver

public class GameManager : SingletonPersistent<GameManager>
{
    public GameState State { get; private set; }
    public event Action<GameState> OnStateChanged;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            LoadScene(1);
    }

    private void OnEnable()
    {
        if (InputHandler.Instance != null)
            InputHandler.Instance.OnPause += TogglePause;
    }

    private void OnDisable()
    {
        if (InputHandler.Instance != null)
            InputHandler.Instance.OnPause -= TogglePause;
    }

    public void ChangeState(GameState newState)
    {
        State = newState;
        Time.timeScale = (State == GameState.Paused || State == GameState.GameOver) ? 0f : 1f;
        OnStateChanged?.Invoke(newState);
    }

    public void TogglePause()
    {
        // Block pause input during game over or menus
        if (State == GameState.GameOver || State == GameState.MainMenu) return;

        if (State == GameState.Playing)
            ChangeState(GameState.Paused);
        else if (State == GameState.Paused)
            ChangeState(GameState.Playing);
    }

    // 👇 NEW: called by HealthSystem via event
    public void TriggerGameOver()
    {
        if (State == GameState.GameOver) return; // prevent double-trigger
        ChangeState(GameState.GameOver);
    }

    public void LoadScene(int buildIndex)
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(buildIndex).completed += (_) =>
        {
            if (buildIndex == 1 || buildIndex == 2)
                ChangeState(GameState.MainMenu);
            else
                ChangeState(GameState.Playing);
        };
    }

    public void SoftResetLevel()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}