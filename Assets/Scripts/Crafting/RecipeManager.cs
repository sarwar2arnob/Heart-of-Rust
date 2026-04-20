using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance;

    private HashSet<RecipeData> unlockedRecipes = new();

    private void Awake()
    {
        Instance = this;
    }

    public void UnlockRecipe(RecipeData recipe)
    {
        if (recipe == null) return;
        if (unlockedRecipes.Contains(recipe)) return;

        unlockedRecipes.Add(recipe);
        Debug.Log($"Recipe Unlocked: {recipe.name}");
    }

    public bool IsUnlocked(RecipeData recipe)
    {
        return recipe.unlockedByDefault || unlockedRecipes.Contains(recipe);
    }
}