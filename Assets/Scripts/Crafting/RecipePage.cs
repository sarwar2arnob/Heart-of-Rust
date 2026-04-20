using UnityEngine;

public class RecipePage : MonoBehaviour, IInteractable
{
    public RecipeData recipeToUnlock;

    public void Interact(PlayerController player)
    {
        RecipeManager.Instance.UnlockRecipe(recipeToUnlock);

        Debug.Log($"Learned recipe: {recipeToUnlock.name}");

        Destroy(gameObject);
    }
}