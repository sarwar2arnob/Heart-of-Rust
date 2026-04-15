using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [Header("Database")]
    public List<RecipeData> recipes;

    // The Observable event that other systems (like UI or Player) can listen to
    public event Action<CraftResult> OnCraftSuccess;

    public bool TryCraft(List<ItemAmount> input, InventorySystem inventory)
    {
        foreach (var recipe in recipes)
        {
            if (Match(recipe, input))
            {
                Consume(recipe, inventory);
                OnCraftSuccess?.Invoke(recipe.result);
                return true;
            }
        }

        Debug.Log("Craft Failed: No matching recipe found or insufficient quantities.");
        return false;
    }

    private bool Match(RecipeData recipe, List<ItemAmount> input)
    {
        // 1. If the number of distinct item slots doesn't match, it's not this recipe
        if (recipe.inputs.Count != input.Count) return false;

        // 2. Verify every required item and its exact amount exists in the input list
        foreach (var recipeItem in recipe.inputs)
        {
            bool foundMatch = false;
            foreach (var inputItem in input)
            {
                if (recipeItem.item == inputItem.item && recipeItem.amount == inputItem.amount)
                {
                    foundMatch = true;
                    break;
                }
            }

            if (!foundMatch) return false;
        }

        return true;
    }

    private void Consume(RecipeData recipe, InventorySystem inventory)
    {
        foreach (var item in recipe.inputs)
        {
            inventory.Remove(item.item, item.amount);
        }
    }
}