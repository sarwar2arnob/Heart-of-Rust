using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverlayUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Pause Buttons")]
    [SerializeField] private Button pauseResumeButton;
    [SerializeField] private Button pauseRestartButton;

    [Header("Game Over Buttons")]
    [SerializeField] private Button gameOverRestartButton;

    [Header("Optional")]
    [SerializeField] private TMP_Text gameOverTitleText;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged += HandleStateChanged;
        }

        pauseResumeButton.onClick.AddListener(OnResumeClicked);
        pauseRestartButton.onClick.AddListener(OnRestartClicked);

        gameOverRestartButton.onClick.AddListener(OnRestartClicked);
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= HandleStateChanged;
        }

        pauseResumeButton.onClick.RemoveAllListeners();
        pauseRestartButton.onClick.RemoveAllListeners();

        gameOverRestartButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    private void HandleStateChanged(GameState newState)
    {
        pausePanel.SetActive(newState == GameState.Paused);
        gameOverPanel.SetActive(newState == GameState.GameOver);

        if (newState == GameState.GameOver &&
            gameOverTitleText != null)
        {
            gameOverTitleText.text = "SYSTEM FAILURE";
        }
    }

    private void OnResumeClicked()
    {
        GameManager.Instance.TogglePause();
    }

    private void OnRestartClicked()
    {
        GameManager.Instance.RestartGame();
    }
}