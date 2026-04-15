using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public List<RecipeData> recipes;

    public bool TryCraft(List<ItemAmount> input, InventorySystem inventory)
    {
        foreach (var recipe in recipes)
        {
            if (Match(recipe, input))
            {
                Consume(recipe, inventory);
                ApplyResult(recipe.result);
                return true;
            }
        }

        Debug.Log("Craft Failed");
        return false;
    }

    bool Match(RecipeData recipe, List<ItemAmount> input)
    {
        // Compare items + amounts
        return true; // implement properly
    }

    void Consume(RecipeData recipe, InventorySystem inventory)
    {
        foreach (var item in recipe.inputs)
            inventory.Remove(item.item, item.amount);
    }

    void ApplyResult(CraftResult result)
    {
        // Find the PlayerEquipment component instead of PlayerState
        var playerEquipment = Object.FindFirstObjectByType<PlayerEquipment>();

        if (playerEquipment != null)
        {
            if (result.type == ResultType.Module)
            {
                playerEquipment.EquipModule(result.module);
            }
            else if (result.type == ResultType.Part)
            {
                playerEquipment.UnlockPart(result.part);
            }
        }
        else
        {
            Debug.LogError("PlayerEquipment not found in the scene!");
        }
    }
}