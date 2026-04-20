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
        var inventory = FindAnyObjectByType<InventorySystem>();
        var ui = FindAnyObjectByType<CraftingUI_Slots>(FindObjectsInactive.Include); // <-- fix

        if (ui == null)
        {
            Debug.LogError("[Toolbox] CraftingUI_Slots not found in scene!");
            return;
        }

        ui.Open(craftingSystem, inventory, player.equipment);
        player.StateMachine.ChangeState(player.CraftingState);
    }
}