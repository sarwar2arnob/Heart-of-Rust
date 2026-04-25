using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingSlotUI : MonoBehaviour
{
    [Header("Setup in Inspector")]
    public ItemData itemData; // Assign Scrap, Core, or Wire here in the Inspector
    [SerializeField] private Image slotImage;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;

    public int CurrentCount { get; private set; }
    private InventorySystem inventory;

    private void Start()
    {
        plusButton.onClick.AddListener(OnPlus);
        minusButton.onClick.AddListener(OnMinus);
        if (itemData != null && slotImage != null)
        {
            slotImage.sprite = itemData.icon;
            slotImage.enabled = true;
        }
    }

    public void Init(InventorySystem inv)
    {
        inventory = inv;
        ResetSlot();
    }

    private void OnPlus()
    {
        if (inventory == null) return;

        // Check if player actually has enough in their inventory to keep adding
        if (CurrentCount < inventory.GetCount(itemData))
        {
            CurrentCount++;
            UpdateUI();
        }
        else
        {
            AudioManager.Instance?.Play(SoundType.CraftFail); // Out of items!
        }
    }

    private void OnMinus()
    {
        if (CurrentCount > 0)
        {
            CurrentCount--;
            UpdateUI();
        }
    }

    public void ResetSlot()
    {
        CurrentCount = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        countText.text = CurrentCount.ToString();
    }
}