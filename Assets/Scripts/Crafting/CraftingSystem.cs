using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public List<RecipeData> recipes;

    public event Action<CraftResult> OnCraftSuccess;

    public RecipeData FindMatchingRecipe(List<ItemData> inputs)
    {
        foreach (var recipe in recipes)
        {
            if (!RecipeManager.Instance.IsUnlocked(recipe))
                continue;

            if (Matches(recipe, inputs))
                return recipe;
        }
        return null;
    }

    private bool Matches(RecipeData recipe, List<ItemData> inputs)
    {
        List<ItemData> required = new();

        foreach (var i in recipe.inputs)
        {
            for (int j = 0; j < i.amount; j++)
                required.Add(i.item);
        }

        if (required.Count != inputs.Count)
            return false;

        foreach (var input in inputs)
        {
            if (!required.Contains(input))
                return false;

            required.Remove(input);
        }

        return required.Count == 0;
    }

    public bool CanCraft(RecipeData recipe, InventorySystem inventory)
    {
        foreach (var item in recipe.inputs)
        {
            if (!inventory.Has(item.item, item.amount))
                return false;
        }
        return true;
    }

    public bool TryCraft(RecipeData recipe, InventorySystem inventory)
    {
        if (!CanCraft(recipe, inventory))
            return false;

        foreach (var item in recipe.inputs)
        {
            inventory.Remove(item.item, item.amount);
        }

        OnCraftSuccess?.Invoke(recipe.result);
        return true;
    }
}