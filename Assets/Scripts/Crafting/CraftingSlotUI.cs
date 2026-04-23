using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingSlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Button addButton;
    [SerializeField] private Button removeButton;

    public ItemData CurrentItem { get; private set; }
    public int Count { get; private set; }

    private InventorySystem inventory;

    public void Setup(InventorySystem inv)
    {
        inventory = inv;

        addButton.onClick.RemoveAllListeners();
        removeButton.onClick.RemoveAllListeners();
        addButton.onClick.AddListener(OnAdd);
        removeButton.onClick.AddListener(OnRemove);

        Clear();
    }

    public void Init()
    {
        Clear();
    }

    public void SetItem(ItemData item)
    {
        CurrentItem = item;
        Count = 1;
        icon.sprite = item.icon;
        icon.enabled = true;
        UpdateUI();
    }

    private void OnAdd()
    {
        if (CurrentItem == null) return;

        // Check if the player has enough items
        if (!inventory.Has(CurrentItem, Count + 1))
        {
            Debug.Log($"[CraftingSlotUI] Cannot add more {CurrentItem.itemName}. Not enough in inventory!");

            // Optional: Play your fail sound here!
            if (AudioManager.Instance != null)
                AudioManager.Instance.Play(SoundType.CraftFail);

            return;
        }

        Count++;
        UpdateUI();
    }
    private void OnRemove()
    {
        if (CurrentItem == null) return;

        Count--;

        if (Count <= 0)
        {
            Clear();
            return;
        }

        UpdateUI();
    }

    public void Clear()
    {
        CurrentItem = null;
        Count = 0;
        icon.enabled = false;
        countText.text = "";
    }

    public bool IsEmpty() => CurrentItem == null;

    private void UpdateUI()
    {
        countText.text = Count.ToString();
    }
}