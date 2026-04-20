using UnityEngine;
using UnityEngine.UI;

public class CraftingSlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;

    public ItemData CurrentItem { get; private set; }

    public void SetItem(ItemData item)
    {
        CurrentItem = item;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void Clear()
    {
        CurrentItem = null;
        icon.enabled = false;
    }

    public bool IsEmpty() => CurrentItem == null;
}