using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private Dictionary<ItemData, int> items =
        new Dictionary<ItemData, int>();

    public event Action OnInventoryChanged;

    public void Add(ItemData item, int amount = 1)
    {
        if (item == null)
            return;

        if (items.ContainsKey(item))
            items[item] += amount;
        else
            items[item] = amount;

        OnInventoryChanged?.Invoke();
    }

    public bool Remove(ItemData item, int amount = 1)
    {
        if (item == null)
            return false;

        if (!items.ContainsKey(item))
            return false;

        if (items[item] < amount)
            return false;

        items[item] -= amount;

        if (items[item] <= 0)
            items.Remove(item);

        OnInventoryChanged?.Invoke();

        return true;
    }

    public bool Has(ItemData item, int amount = 1)
    {
        if (item == null)
            return false;

        if (!items.TryGetValue(item, out int count))
            return false;

        return count >= amount;
    }

    public int GetCount(ItemData item)
    {
        if (item == null)
            return 0;

        if (items.TryGetValue(item, out int count))
            return count;

        return 0;
    }

    public Dictionary<ItemData, int> GetAllItems()
    {
        return items;
    }

    public void Clear()
    {
        items.Clear();

        OnInventoryChanged?.Invoke();
    }
}