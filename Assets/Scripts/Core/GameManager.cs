using UnityEngine;
using UnityEngine.SceneManagement;
using System;

// 1. Add Victory to the Enum
public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver,
    Victory
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State { get; private set; }

    public event Action<GameState> OnStateChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ChangeState(GameState newState)
    {
        State = newState;

        // 2. Pause time if Game Over or Victory
        Time.timeScale =
            (State == GameState.Paused || State == GameState.GameOver || State == GameState.Victory)
            ? 0f : 1f;

        OnStateChanged?.Invoke(newState);
    }

    public void TogglePause()
    {
        if (State == GameState.MainMenu || State == GameState.GameOver || State == GameState.Victory)
            return;

        if (State == GameState.Playing) ChangeState(GameState.Paused);
        else if (State == GameState.Paused) ChangeState(GameState.Playing);
    }

    public void TriggerGameOver()
    {
        if (State == GameState.GameOver) return;
        ChangeState(GameState.GameOver);
    }

    // 3. New Trigger for the Door
    public void TriggerVictory()
    {
        if (State == GameState.Victory) return;
        ChangeState(GameState.Victory);
    }

    public void RestartGame()
    {
        ReturnToMainMenu(); // Reroute restart to the main menu function
    }

    // 4. Cleanly returns to the main menu by resetting the scene
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}