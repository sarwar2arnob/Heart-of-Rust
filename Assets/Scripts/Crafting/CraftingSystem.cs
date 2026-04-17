using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [Header("Database")]
    public List<RecipeData> recipes;

    public event Action<CraftResult> OnCraftSuccess;

    public bool TryCraft(RecipeData recipe, InventorySystem inventory)
    {
        if (!CanCraft(recipe, inventory))
        {
            Debug.Log("Not enough materials!");
            return false;
        }

        Consume(recipe, inventory);

        OnCraftSuccess?.Invoke(recipe.result);

        return true;
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

    private void Consume(RecipeData recipe, InventorySystem inventory)
    {
        foreach (var item in recipe.inputs)
        {
            inventory.Remove(item.item, item.amount);
        }
    }
}