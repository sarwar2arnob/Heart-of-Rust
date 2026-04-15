using UnityEngine;

public interface IInteractable
{
    // Notice we pass the PlayerController, not PlayerState
    void Interact(PlayerController player);
}