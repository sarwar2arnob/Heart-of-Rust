// World/LevelDoor.cs

using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    // Set this in the Inspector for each door in each scene
    [SerializeField] private int nextLevelBuildIndex;

    private bool hasTriggered = false; // prevent double-fire

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        // Only the player opens the door
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;

        LevelManager levelManager = FindFirstObjectByType<LevelManager>();

        if (levelManager != null)
        {
            levelManager.CompleteLevel(nextLevelBuildIndex);
        }
        else
        {
            // Fallback: no LevelManager in scene, go directly
            Debug.LogWarning("[LevelDoor] No LevelManager found, loading directly.");
            SaveManager.Instance.UnlockLevel(nextLevelBuildIndex);
            GameManager.Instance.LoadScene(nextLevelBuildIndex);
        }
    }

    // Visualize trigger radius in editor
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0.4f, 0.4f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}