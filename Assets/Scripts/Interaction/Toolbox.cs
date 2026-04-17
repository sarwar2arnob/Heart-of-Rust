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
        craftingSystem.OnCraftSuccess += HandleSuccessfulCraft;
    }

    void OnDisable()
    {
        craftingSystem.OnCraftSuccess -= HandleSuccessfulCraft;
    }

    public void Interact(PlayerController player)
    {
        currentPlayerEquipment = player.equipment;

        player.StateMachine.ChangeState(player.CraftingState);

        // 🔥 Open Crafting UI
        CraftingUI.Instance.Open(this, craftingSystem);

        Debug.Log("Toolbox Opened!");
    }

    private void HandleSuccessfulCraft(CraftResult result)
    {
        if (currentPlayerEquipment != null)
        {
            currentPlayerEquipment.ApplyCraftResult(result);
            Debug.Log("Craft Successful! Applied to Player.");
        }
    }
}