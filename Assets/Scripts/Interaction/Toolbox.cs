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


    public void Interact(PlayerController player)
    {
        var inventory = FindFirstObjectByType<InventorySystem>();

        // Warn us if the UI is missing or turned off!
        if (CraftingUI_Slots.Instance == null)
        {
            Debug.LogError("[Toolbox] CraftingUI_Slots Instance is NULL! Make sure the GameObject is active in the scene so Awake() can run.");
            return; // Stop the code here
        }

        OnOpenUI.Invoke();

        // Pass the required references
        CraftingUI_Slots.Instance.Open(this, craftingSystem, inventory, player);
        player.StateMachine.ChangeState(player.CraftingState);
    }
}