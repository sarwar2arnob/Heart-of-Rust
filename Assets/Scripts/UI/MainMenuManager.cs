using UnityEngine;
using UnityEngine.UI;
using TMPro; // Required for TextMeshPro

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelSelectPanel;

    [Header("Main Menu Buttons")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button levelSelectButton;

    [Header("Level Select Setup")]
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject levelButtonPrefab;  // The button we will spawn
    [SerializeField] private Transform levelGridContainer;  // The UI Panel with the GridLayoutGroup

    [Header("Configuration")]
    [SerializeField] private int totalLevels = 12;
    [SerializeField] private int levelSceneOffset = 1;

    private void Start()
    {
        continueButton.onClick.AddListener(OnContinueClicked);
        levelSelectButton.onClick.AddListener(OnLevelSelectClicked);
        backButton.onClick.AddListener(OnBackClicked);

        SetupLevelGrid();
        ShowPanel(mainMenuPanel);
    }

    private void SetupLevelGrid()
    {
        int highestUnlocked = SaveManager.Instance.CurrentData.highestUnlockedLevel;

        // Dynamically spawn all 12 buttons
        for (int i = 0; i < totalLevels; i++)
        {
            int levelNumber = i + 1;

            // 1. Spawn the button inside the grid container
            GameObject newButtonObj = Instantiate(levelButtonPrefab, levelGridContainer);
            Button buttonComponent = newButtonObj.GetComponent<Button>();

            // 2. Set the text to the level number
            TMP_Text buttonText = newButtonObj.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = levelNumber.ToString();
            }

            // 3. Apply locking logic
            if (levelNumber <= highestUnlocked)
            {
                // Unlocked
                buttonComponent.interactable = true;
                int sceneToLoad = levelNumber + levelSceneOffset;
                buttonComponent.onClick.AddListener(() => GameManager.Instance.LoadScene(sceneToLoad));
            }
            else
            {
                // Locked
                buttonComponent.interactable = false;
            }
        }
    }

    private void OnContinueClicked()
    {
        int highestUnlocked = SaveManager.Instance.CurrentData.highestUnlockedLevel;
        GameManager.Instance.LoadScene(highestUnlocked + levelSceneOffset);
    }

    private void OnLevelSelectClicked()
    {
        ShowPanel(levelSelectPanel);
    }

    private void OnBackClicked()
    {
        ShowPanel(mainMenuPanel);
    }

    private void ShowPanel(GameObject panelToShow)
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        panelToShow.SetActive(true);
    }
}