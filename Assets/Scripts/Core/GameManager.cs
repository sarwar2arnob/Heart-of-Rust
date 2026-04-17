using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Singleton;

public enum GameState { Boot, MainMenu, Playing, Paused }

public class GameManager : SingletonPersistent<GameManager>
{
    public GameState State { get; private set; }

    // Events for UI scripts to listen to
    public event Action<GameState> OnStateChanged;

    private void Start()
    {
        // If we started in the Boot scene, transition to Main Menu
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            LoadScene(1); // Load MainMenu
        }
    }

    private void OnEnable()
    {
        // Subscribe to your existing InputHandler pause event!
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

        // Handle Time Scale for pausing
        if (State == GameState.Paused)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;

        OnStateChanged?.Invoke(newState);
    }

    public void TogglePause()
    {
        if (State == GameState.Playing)
            ChangeState(GameState.Paused);
        else if (State == GameState.Paused)
            ChangeState(GameState.Playing);
    }

    // --- Scene Management ---

    public void LoadScene(int buildIndex)
    {
        // Async loading prevents WebGL freezing during scene transitions
        Time.timeScale = 1f; // Always ensure time is running when loading
        SceneManager.LoadSceneAsync(buildIndex).completed += (asyncOperation) =>
        {
            if (buildIndex == 1 || buildIndex == 2) // MainMenu or LevelSelect
                ChangeState(GameState.MainMenu);
            else
                ChangeState(GameState.Playing);
        };
    }

    public void SoftResetLevel()
    {
        // Reloads the current active scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(currentSceneIndex);
    }
}