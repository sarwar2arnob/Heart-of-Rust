using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text countText;

    public void Setup(ItemData item, int count)
    {
        icon.sprite = item.icon;
        countText.text = count.ToString();
    }
}