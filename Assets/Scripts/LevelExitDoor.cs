using UnityEngine;

public class LevelExitDoor : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public bool isUnlocked = true; // Set this to false if they need a key or to finish a puzzle first

    public void Interact(PlayerController player)
    {
        if (isUnlocked)
        {
            Debug.Log("Door opened! Level Complete!");
            GameManager.Instance.TriggerVictory();
        }
        else
        {
            Debug.Log("The door is locked.");
            // Optional: If you have a ClueManager, tell the player it's locked here
        }
    }
}