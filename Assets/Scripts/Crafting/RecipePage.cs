using UnityEngine;

public class RecipePage : MonoBehaviour, IInteractable
{
    public RecipeData recipeToUnlock;

    public void Interact(PlayerController player)
    {
        // Unlock immediately — player has seen/touched it
        RecipeManager.Instance.UnlockRecipe(recipeToUnlock);

        // Show the UI — it will destroy THIS gameObject after dismiss
        if (RecipePageUI.Instance != null)
            RecipePageUI.Instance.Show(recipeToUnlock, gameObject);
        else
        {
            // Fallback if no UI in scene
            Debug.LogWarning("[RecipePage] RecipePageUI not found, destroying immediately.");
            Destroy(gameObject);
        }
    }
}