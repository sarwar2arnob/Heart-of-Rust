// UI/RecipePageUI.cs

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipePageUI : MonoBehaviour
{
    public static RecipePageUI Instance;

    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Content")]
    [SerializeField] private Image recipeImage;
    [SerializeField] private TMP_Text recipeNameText;
    [SerializeField] private TMP_Text ingredientsText;

    [Header("Buttons")]
    [SerializeField] private Button gotItButton;

    // The world object waiting to be destroyed after dismiss
    private GameObject pendingDestroy;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    private void Start()
    {
        gotItButton.onClick.AddListener(OnGotItClicked);
    }

    public void Show(RecipeData recipe, GameObject worldObject)
    {
        pendingDestroy = worldObject;

        // Fill content
        recipeNameText.text = recipe.recipeName;
        recipeImage.sprite = recipe.recipePageImage;
        recipeImage.enabled = recipe.recipePageImage != null;

        // Build ingredients list
        string ingredients = "<b>Requires:</b>\n";
        foreach (var item in recipe.inputs)
            ingredients += $"• {item.item.itemName} x{item.amount}\n";
        ingredientsText.text = ingredients;

        panel.SetActive(true);

        // Pause the game while reading
        GameManager.Instance?.ChangeState(GameState.Paused);
    }

    private void OnGotItClicked()
    {
        panel.SetActive(false);

        // Resume before destroying
        GameManager.Instance?.ChangeState(GameState.Playing);

        // NOW destroy the world object
        if (pendingDestroy != null)
        {
            Destroy(pendingDestroy);
            pendingDestroy = null;
        }
    }
}