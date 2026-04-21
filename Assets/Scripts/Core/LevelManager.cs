// Core/LevelManager.cs

using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Setup")]
    public string clueText;
    public int nextLevelBuildIndex; // set this in the Inspector per-scene

    private void Start()
    {
        // Null-safe clue display
        ClueManager clueManager = FindFirstObjectByType<ClueManager>();
        if (clueManager != null && !string.IsNullOrEmpty(clueText))
            clueManager.ShowClue(clueText);
    }

    // Call this from a trigger, door, or puzzle-complete event
    public void CompleteLevel(int nextLevelBuildIndex)
    {
        SaveManager.Instance.UnlockLevel(nextLevelBuildIndex);
        GameManager.Instance.LoadScene(nextLevelBuildIndex);
    }
}