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
    [SerializeField] private Button pauseMainMenuButton;

    [Header("Game Over Buttons")]
    [SerializeField] private Button gameOverRestartButton;
    [SerializeField] private Button gameOverMainMenuButton;

    // Optional: show a reason on the game over screen
    [Header("Game Over Text (optional)")]
    [SerializeField] private TMP_Text gameOverTitleText;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged += HandleStateChanged;

        pauseResumeButton.onClick.AddListener(OnResumeClicked);
        pauseRestartButton.onClick.AddListener(OnRestartClicked);
        pauseMainMenuButton.onClick.AddListener(OnMainMenuClicked);

        gameOverRestartButton.onClick.AddListener(OnRestartClicked);
        gameOverMainMenuButton.onClick.AddListener(OnMainMenuClicked);
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged -= HandleStateChanged;

        pauseResumeButton.onClick.RemoveAllListeners();
        pauseRestartButton.onClick.RemoveAllListeners();
        pauseMainMenuButton.onClick.RemoveAllListeners();
        gameOverRestartButton.onClick.RemoveAllListeners();
        gameOverMainMenuButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    private void HandleStateChanged(GameState newState)
    {
        // Hide both first, then show the right one
        pausePanel.SetActive(newState == GameState.Paused);
        gameOverPanel.SetActive(newState == GameState.GameOver);

        if (newState == GameState.GameOver && gameOverTitleText != null)
            gameOverTitleText.text = "SYSTEM FAILURE"; // or whatever fits your game
    }

    private void OnResumeClicked() => GameManager.Instance.TogglePause();
    private void OnRestartClicked() => GameManager.Instance.SoftResetLevel();
    private void OnMainMenuClicked() => GameManager.Instance.LoadScene(1);
}