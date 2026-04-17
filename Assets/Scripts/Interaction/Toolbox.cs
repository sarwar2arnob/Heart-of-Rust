using UnityEngine;

[RequireComponent(typeof(CraftingSystem))]
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
        // Listen for successful crafts from the attached CraftingSystem
        craftingSystem.OnCraftSuccess += HandleSuccessfulCraft;
    }

    void OnDisable()
    {
        craftingSystem.OnCraftSuccess -= HandleSuccessfulCraft;
    }

    // Called by the PlayerController when they press the Interact key nearby
    public void Interact(PlayerController player)
    {
        // Cache the player's equipment when they open the toolbox
        currentPlayerEquipment = player.equipment;

        // 1. Tell the player's state machine to freeze them
        player.StateMachine.ChangeState(player.CraftingState);

        // 2. Trigger your UI event here! 
        // InputHandler.Instance.OnCraftingOpen?.Invoke(); 
        Debug.Log("Toolbox Opened! Player frozen in CraftingState.");
    }

    private void HandleSuccessfulCraft(CraftResult result)
    {
        // Forward the new part/module directly to the player's equipment
        if (currentPlayerEquipment != null)
        {
            currentPlayerEquipment.ApplyCraftResult(result);

            // Trigger visual/audio feedback for crafting success
            Debug.Log("Craft Successful! Applied to Chassis.");
        }
    }
}
