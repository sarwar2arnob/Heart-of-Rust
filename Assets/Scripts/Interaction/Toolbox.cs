using UnityEngine;

public class Toolbox : MonoBehaviour, IInteractable
{
    private CraftingSystem craftingSystem;
    private PlayerEquipment currentPlayerEquipment;

    void Awake()
    {
        craftingSystem = GetComponent<CraftingSystem>();
    }

    void OnEnable()
    {
        // Listen for successful crafts
        craftingSystem.OnCraftSuccess += HandleSuccessfulCraft;
    }

    void OnDisable()
    {
        craftingSystem.OnCraftSuccess -= HandleSuccessfulCraft;
    }

    public void Interact(PlayerController player)
    {
        // Cache the player's equipment when they open the toolbox
        currentPlayerEquipment = player.equipment;
        // Trigger your UI opening logic here
    }

    private void HandleSuccessfulCraft(CraftResult result)
    {
        if (currentPlayerEquipment != null)
        {
            currentPlayerEquipment.ApplyCraftResult(result);
            // Trigger visual/audio feedback here
        }
    }
}