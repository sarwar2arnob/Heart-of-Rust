using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Button button; // OPTIONAL

    private ItemData currentItem;

    public void Setup(ItemData item, int count)
    {
        currentItem = item;

        icon.sprite = item.icon;
        countText.text = count.ToString();

        // ✅ Only if button exists
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }
    }

    private void OnClick()
    {
    }
}