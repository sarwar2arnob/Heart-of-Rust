using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [Header("All Game Recipes")]
    public List<RecipeData> recipes;

    public event Action<CraftResult> OnCraftSuccess;

    // Checks if the items in the slots perfectly match any recipe
    public RecipeData FindMatchingRecipe(List<ItemData> inputtedItems)
    {
        foreach (var recipe in recipes)
        {
            if (Matches(recipe, inputtedItems))
                return recipe;
        }
        return null; // No match found
    }

    // Validates the exact quantities
    private bool Matches(RecipeData recipe, List<ItemData> inputtedItems)
    {
        // 1. Create a dictionary of what the recipe requires
        Dictionary<ItemData, int> requiredCounts = new Dictionary<ItemData, int>();
        foreach (var req in recipe.inputs)
        {
            requiredCounts[req.item] = req.amount;
        }

        // 2. Create a dictionary of what the player put in the slots
        Dictionary<ItemData, int> inputCounts = new Dictionary<ItemData, int>();
        foreach (var item in inputtedItems)
        {
            if (inputCounts.ContainsKey(item))
                inputCounts[item]++;
            else
                inputCounts[item] = 1;
        }

        // 3. Ensure counts match exactly (no missing items, no extra junk items)
        if (requiredCounts.Count != inputCounts.Count) return false;

        foreach (var kvp in requiredCounts)
        {
            if (!inputCounts.ContainsKey(kvp.Key)) return false;
            if (inputCounts[kvp.Key] != kvp.Value) return false;
        }

        return true; // Perfect match!
    }

    // Actually consumes the items and grants the reward
    public void PerformCraft(RecipeData recipe, InventorySystem inventory)
    {
        // Remove the items from the real inventory
        foreach (var item in recipe.inputs)
        {
            inventory.Remove(item.item, item.amount);
        }

        // Fire the event to give the player the module/part
        OnCraftSuccess?.Invoke(recipe.result);

        // Optional: Show your popup
        if (CraftResultPopup.Instance != null)
            CraftResultPopup.Instance.Show(recipe);
    }
}