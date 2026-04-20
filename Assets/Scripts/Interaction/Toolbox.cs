using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CraftingSystem))]
public class Toolbox : MonoBehaviour, IInteractable
{
    private CraftingSystem craftingSystem;
    public UnityEvent OnOpenUI;

    void Awake()
    {
        craftingSystem = GetComponent<CraftingSystem>();
    }

    void Start()
    {
        var inventory = FindAnyObjectByType<InventorySystem>();
        var ui = FindAnyObjectByType<CraftingUI_Slots>(FindObjectsInactive.Include);

        if (ui != null && inventory != null)
            ui.Setup(inventory);
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
        OnOpenUI.Invoke();
        ui.Open(craftingSystem, inventory, player.equipment);
        player.StateMachine.ChangeState(player.CraftingState);
    }
}