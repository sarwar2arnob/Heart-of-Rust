using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftingUI_Slots : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField] private CraftingSlotUI slot1;
    [SerializeField] private CraftingSlotUI slot2;

    [Header("UI")]
    [SerializeField] private TMP_Text feedbackText;

    private CraftingSystem craftingSystem;
    private InventorySystem inventory;
    private PlayerEquipment playerEquipment;

    public void Open(CraftingSystem system, InventorySystem inv, PlayerEquipment equipment)
    {
        craftingSystem = system;
        inventory = inv;
        playerEquipment = equipment;

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        ClearSlots();
    }

    public void TryCraft()
    {
        List<ItemData> inputs = new();

        if (!slot1.IsEmpty()) inputs.Add(slot1.CurrentItem);
        if (!slot2.IsEmpty()) inputs.Add(slot2.CurrentItem);

        if (inputs.Count == 0)
        {
            feedbackText.text = "Insert items!";
            return;
        }

        RecipeData recipe = craftingSystem.FindMatchingRecipe(inputs);

        if (recipe == null)
        {
            feedbackText.text = "Unknown combination...";
            return;
        }

        if (!craftingSystem.CanCraft(recipe, inventory))
        {
            feedbackText.text = "Not enough materials!";
            return;
        }

        craftingSystem.TryCraft(recipe, inventory);

        feedbackText.text = "Crafted!";
        ClearSlots();
    }

    private void ClearSlots()
    {
        slot1.Clear();
        slot2.Clear();
    }
}