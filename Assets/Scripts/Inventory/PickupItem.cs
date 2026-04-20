using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    public ItemData itemData;

    public void Interact(PlayerController player)
    {
        // 1. Add to inventory
        FindFirstObjectByType<InventorySystem>().Add(itemData);

        // 2. Play a quick pickup animation but DON'T change the state (player can keep walking)
        player.AnimManager.TriggerInteract();

        // 3. Destroy the scrap metal from the world
        Destroy(gameObject);
    }
}