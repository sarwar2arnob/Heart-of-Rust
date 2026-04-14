using UnityEngine;
using System.Collections.Generic;

public class InventorySystem : MonoBehaviour
{
    public int maxSlots = 10;

    private Dictionary<ItemData, int> items = new();

    public void Add(ItemData item)
    {
        if (!items.ContainsKey(item))
            items[item] = 0;

        items[item]++;
    }

    public bool Has(ItemData item, int amount)
    {
        return items.ContainsKey(item) && items[item] >= amount;
    }

    public void Remove(ItemData item, int amount)
    {
        if (!Has(item, amount)) return;

        items[item] -= amount;
    }
}