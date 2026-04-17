using UnityEngine;
using System;
using System.Collections.Generic;

public class InventorySystem : MonoBehaviour
{
    public int maxSlots = 10;

    private Dictionary<ItemData, int> items = new();

    // 🔥 Event for UI updates
    public event Action OnInventoryChanged;

    public void Add(ItemData item)
    {
        if (!items.ContainsKey(item))
            items[item] = 0;

        items[item]++;

        OnInventoryChanged?.Invoke();
    }

    public bool Has(ItemData item, int amount)
    {
        return items.ContainsKey(item) && items[item] >= amount;
    }

    public void Remove(ItemData item, int amount)
    {
        if (!Has(item, amount)) return;

        items[item] -= amount;

        // Optional: remove empty entries
        if (items[item] <= 0)
            items.Remove(item);

        OnInventoryChanged?.Invoke();
    }

    // 🔥 UI reads from this
    public Dictionary<ItemData, int> GetAllItems()
    {
        return items;
    }
}