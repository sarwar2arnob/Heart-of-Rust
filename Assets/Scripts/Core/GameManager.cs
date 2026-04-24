using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State { get; private set; }

    public event Action<GameState> OnStateChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeState(GameState newState)
    {
        State = newState;

        Time.timeScale =
            (State == GameState.Paused ||
             State == GameState.GameOver)
            ? 0f
            : 1f;

        OnStateChanged?.Invoke(newState);
    }

    public void TogglePause()
    {
        if (State == GameState.MainMenu ||
            State == GameState.GameOver)
            return;

        if (State == GameState.Playing)
            ChangeState(GameState.Paused);
        else if (State == GameState.Paused)
            ChangeState(GameState.Playing);
    }

    public void TriggerGameOver()
    {
        if (State == GameState.GameOver)
            return;

        ChangeState(GameState.GameOver);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}