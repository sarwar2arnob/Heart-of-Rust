using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform gridParent;

    private InventorySystem inventory;

    void Start()
    {
        inventory = FindFirstObjectByType<InventorySystem>();

        if (inventory == null)
        {
            Debug.LogError("InventorySystem not found in scene!");
            return;
        }

        inventory.OnInventoryChanged += RefreshUI;

        RefreshUI();
    }

    void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged -= RefreshUI;
        }
    }

    void RefreshUI()
    {
        // Clear old UI
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        // Rebuild UI
        Dictionary<ItemData, int> items = inventory.GetAllItems();

        foreach (var pair in items)
        {
            GameObject slot = Instantiate(slotPrefab, gridParent);

            InventorySlotUI slotUI = slot.GetComponent<InventorySlotUI>();
            slotUI.Setup(pair.Key, pair.Value);
        }
    }
}