using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI_Slots : MonoBehaviour
{
    public static CraftingUI_Slots Instance;

    [Header("UI Elements")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Button craftButton;
    [SerializeField] private Button backButton;

    [Header("The Adjusters")]
    [SerializeField] private CraftingSlotUI[] adjusters; // Drag your 3 slot GameObjects here

    private CraftingSystem craftingSystem;
    private InventorySystem inventory;
    private PlayerController player;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    private void Start()
    {
        craftButton.onClick.AddListener(TryCraft);
        backButton.onClick.AddListener(Close);
    }

    // Called by the Toolbox
    public void Open(Toolbox toolbox, CraftingSystem cs, InventorySystem inv, PlayerController pc)
    {
        craftingSystem = cs;
        inventory = inv;
        player = pc;

        // Initialize and reset all 3 adjusters to 0
        foreach (var adjuster in adjusters)
        {
            adjuster.Init(inventory);
        }

        panel.SetActive(true);
    }

    public void Close()
    {
        panel.SetActive(false);
        if (player != null)
        {
            player.StateMachine.ChangeState(player.IdleState);
        }
    }

    private void TryCraft()
    {
        // 1. Gather what the player dialed in
        List<ItemAmount> dialedInputs = new List<ItemAmount>();
        foreach (var adjuster in adjusters)
        {
            if (adjuster.CurrentCount > 0)
            {
                dialedInputs.Add(new ItemAmount { item = adjuster.itemData, amount = adjuster.CurrentCount });
            }
        }

        if (dialedInputs.Count == 0) return; // Nothing dialed in

        // 2. Check for a match
        RecipeData matchedRecipe = FindMatchingRecipe(dialedInputs);

        // INSIDE CraftingUI_Slots.cs -> TryCraft()

        if (matchedRecipe != null)
        {
            Debug.Log($"Crafted: {matchedRecipe.recipeName}");
            AudioManager.Instance?.Play(SoundType.CraftSuccess);

            // Remove items from inventory
            foreach (var input in dialedInputs)
            {
                inventory.Remove(input.item, input.amount);
            }

            // ✅ NEW: Hand the item directly to the player's equipment!
            player.equipment.ApplyCraftResult(matchedRecipe.result);

            // Trigger popup
            if (CraftResultPopup.Instance != null) CraftResultPopup.Instance.Show(matchedRecipe);

            // Reset slots back to 0 for the next craft
            foreach (var adjuster in adjusters) adjuster.ResetSlot();
        }
        else
        {
            Debug.Log("Invalid Recipe!");
            AudioManager.Instance?.Play(SoundType.CraftFail);
        }
    }

    // The logic to check if dialed items perfectly match a recipe
    private RecipeData FindMatchingRecipe(List<ItemAmount> dialedInputs)
    {
        foreach (var recipe in craftingSystem.recipes)
        {
            // 1. Filter out any items in the recipe that have an amount of 0 (safety catch)
            List<ItemAmount> validRequirements = new List<ItemAmount>();
            foreach (var req in recipe.inputs)
            {
                if (req.amount > 0) validRequirements.Add(req);
            }

            // 2. If the amount of distinct items required doesn't match the dials, skip
            if (validRequirements.Count != dialedInputs.Count) continue;

            // 3. Bulletproof deep match
            bool isMatch = true;
            foreach (var req in validRequirements)
            {
                bool foundItemMatch = false;
                foreach (var dialed in dialedInputs)
                {
                    if (dialed.item == req.item && dialed.amount == req.amount)
                    {
                        foundItemMatch = true;
                        break;
                    }
                }

                if (!foundItemMatch)
                {
                    isMatch = false;
                    break; // Missing an item or wrong amount
                }
            }

            // 4. Final Progression Check!
            if (isMatch)
            {
                if (RecipeManager.Instance != null && !RecipeManager.Instance.IsUnlocked(recipe))
                {
                    Debug.LogWarning($"[Crafting] Matched {recipe.recipeName}, but you haven't unlocked this recipe page yet!");
                    return null;
                }
                return recipe; // Perfect match AND unlocked!
            }
        }

        return null; // No match found
    }
}