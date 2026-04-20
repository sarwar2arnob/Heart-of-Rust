using UnityEngine;

[RequireComponent(typeof(CraftingSystem))]
public class Toolbox : MonoBehaviour, IInteractable
{
    private CraftingSystem craftingSystem;

    void Awake()
    {
        craftingSystem = GetComponent<CraftingSystem>();
    }

    public void Interact(PlayerController player)
    {
        var inventory = FindFirstObjectByType<InventorySystem>();
        var ui = FindFirstObjectByType<CraftingUI_Slots>();

        ui.Open(craftingSystem, inventory, player.equipment);

        player.StateMachine.ChangeState(player.CraftingState);
    }
}