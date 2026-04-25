using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverlayUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel; // ✅ Add Victory Panel

    [Header("Pause Buttons")]
    [SerializeField] private Button pauseResumeButton;
    [SerializeField] private Button pauseRestartButton;

    [Header("Game Over Buttons")]
    [SerializeField] private Button gameOverMainMenuButton; // ✅ Changed to Main Menu button

    [Header("Victory Buttons")]
    [SerializeField] private Button victoryMainMenuButton; // ✅ Add Victory Main Menu button

    [Header("Optional")]
    [SerializeField] private TMP_Text gameOverTitleText;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged += HandleStateChanged;

        pauseResumeButton.onClick.AddListener(OnResumeClicked);
        pauseRestartButton.onClick.AddListener(OnMainMenuClicked);

        gameOverMainMenuButton.onClick.AddListener(OnMainMenuClicked);
        victoryMainMenuButton.onClick.AddListener(OnMainMenuClicked); // ✅ Listen for click
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged -= HandleStateChanged;

        pauseResumeButton.onClick.RemoveAllListeners();
        pauseRestartButton.onClick.RemoveAllListeners();
        gameOverMainMenuButton.onClick.RemoveAllListeners();
        victoryMainMenuButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    private void HandleStateChanged(GameState newState)
    {
        pausePanel.SetActive(newState == GameState.Paused);
        gameOverPanel.SetActive(newState == GameState.GameOver);

        if (victoryPanel != null)
            victoryPanel.SetActive(newState == GameState.Victory); // ✅ Show Victory panel

        if (newState == GameState.GameOver && gameOverTitleText != null)
        {
            gameOverTitleText.text = "SYSTEM FAILURE";
        }
    }

    private void OnResumeClicked()
    {
        GameManager.Instance.TogglePause();
    }

    // ✅ New method to handle all main menu routing
    private void OnMainMenuClicked()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
}