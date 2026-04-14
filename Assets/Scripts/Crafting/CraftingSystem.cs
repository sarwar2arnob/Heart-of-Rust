using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.LowLevelPhysics2D.PhysicsQuery;

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
        var player = FindObjectOfType<PlayerState>();

        if (result.type == ResultType.Module)
        {
            player.EquipModule(result.module);
        }
        else if (result.type == ResultType.Part)
        {
            player.UnlockPart(result.part);
        }
    }
}