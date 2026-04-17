using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pausePanel;

    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private void OnEnable()
    {
        // 1. Listen for the GameState changing
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged += HandleStateChanged;
        }

        // 2. Wire up the button clicks via code (cleaner than the Inspector)
        resumeButton.onClick.AddListener(OnResumeClicked);
        restartButton.onClick.AddListener(OnRestartClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuClicked);
    }

    private void OnDisable()
    {
        // Always unsubscribe to prevent memory leaks
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= HandleStateChanged;
        }

        resumeButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        // Ensure the panel is hidden when the scene loads
        pausePanel.SetActive(false);
    }

    // --- State Listener ---

    private void HandleStateChanged(GameState newState)
    {
        // Automatically show or hide the panel based on the GameManager's state
        pausePanel.SetActive(newState == GameState.Paused);
    }

    // --- Button Actions ---

    private void OnResumeClicked()
    {
        // Tells the GameManager to unpause, which will fire the event and hide this UI
        GameManager.Instance.TogglePause();
    }

    private void OnRestartClicked()
    {
        // Soft resets the current scene
        GameManager.Instance.SoftResetLevel();
    }

    private void OnMainMenuClicked()
    {
        // Loads Scene 1 (Main Menu)
        GameManager.Instance.LoadScene(1);
    }
}