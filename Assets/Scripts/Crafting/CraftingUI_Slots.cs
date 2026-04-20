using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftingUI_Slots : MonoBehaviour
{
    [SerializeField] private CraftingSlotUI slot1;
    [SerializeField] private CraftingSlotUI slot2;
    [SerializeField] private CraftingSlotUI slot3;

    [SerializeField] private TMP_Text feedbackText;

    private CraftingSystem craftingSystem;
    private InventorySystem inventory;
    private PlayerEquipment equipment;

    public static CraftingUI_Slots Instance;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Setup(InventorySystem inv)
    {
        inventory = inv;

        slot1.Setup(inv);
        slot2.Setup(inv);
        slot3.Setup(inv);
    }

    public void Open(CraftingSystem system, InventorySystem inv, PlayerEquipment eq)
    {
        craftingSystem = system;
        inventory = inv;
        equipment = eq;

        slot1.Init();
        slot2.Init();
        slot3.Init();

        if (feedbackText != null)
            feedbackText.text = "";

        gameObject.SetActive(true);
    }

    public void AddItemToSlot(ItemData item)
    {
        if (slot1.IsEmpty()) { slot1.SetItem(item); return; }
        if (slot2.IsEmpty()) { slot2.SetItem(item); return; }
        if (slot3.IsEmpty()) { slot3.SetItem(item); return; }

        if (feedbackText != null)
            feedbackText.text = "Slots full!";
    }

    public void TryCraft()
    {
        List<ItemData> inputs = new();

        Add(slot1, inputs);
        Add(slot2, inputs);
        Add(slot3, inputs);

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

    private void Add(CraftingSlotUI slot, List<ItemData> list)
    {
        for (int i = 0; i < slot.Count; i++)
        {
            list.Add(slot.CurrentItem);
        }
    }

    private void ClearSlots()
    {
        slot1.Clear();
        slot2.Clear();
        slot3.Clear();
    }
}