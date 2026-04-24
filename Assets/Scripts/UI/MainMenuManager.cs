using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameplayUI;

    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button quitButton;

    [Header("Gameplay Objects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject levelRoot;

    private void Start()
    {
        startButton.onClick.AddListener(StartNewGame);
        continueButton.onClick.AddListener(ContinueGame);
        quitButton.onClick.AddListener(QuitGame);

        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);

        if (gameplayUI != null)
            gameplayUI.SetActive(false);

        if (player != null)
            player.SetActive(false);

        if (levelRoot != null)
            levelRoot.SetActive(false);

        GameManager.Instance.ChangeState(GameState.MainMenu);
    }

    private void StartNewGame()
    {
        mainMenuPanel.SetActive(false);

        if (gameplayUI != null)
            gameplayUI.SetActive(true);

        if (player != null)
            player.SetActive(true);

        if (levelRoot != null)
            levelRoot.SetActive(true);

        GameManager.Instance.ChangeState(GameState.Playing);
    }

    private void ContinueGame()
    {
        StartNewGame();
    }

    private void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}