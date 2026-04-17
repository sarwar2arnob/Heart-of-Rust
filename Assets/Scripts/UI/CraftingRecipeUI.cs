using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingRecipeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Button craftButton;

    private RecipeData recipe;
    private CraftingUI craftingUI;
    private InventorySystem inventory;
    private CraftingSystem craftingSystem;

    public void Setup(RecipeData recipeData, CraftingUI ui, InventorySystem inv, CraftingSystem cs)
    {
        recipe = recipeData;
        craftingUI = ui;
        inventory = inv;
        craftingSystem = cs;

        nameText.text = GetResultName();
        costText.text = GetCostText();

        RefreshState();

        craftButton.onClick.AddListener(OnCraftClicked);
    }

    void RefreshState()
    {
        bool canCraft = craftingSystem.CanCraft(recipe, inventory);

        craftButton.interactable = canCraft;

        // Optional visual feedback
        nameText.color = canCraft ? Color.white : Color.gray;
    }

    void OnCraftClicked()
    {
        craftingUI.TryCraft(recipe);
    }

    string GetResultName()
    {
        if (recipe.result.type == ResultType.Module)
            return recipe.result.module.type.ToString();

        return recipe.result.part.ToString();
    }

    string GetCostText()
    {
        string text = "";

        foreach (var item in recipe.inputs)
        {
            text += $"{item.item.itemName} x{item.amount}\n";
        }

        return text;
    }
}