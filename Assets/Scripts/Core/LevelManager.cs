using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public string clueText;

    void Start()
    {
        FindObjectOfType<ClueManager>().ShowClue(clueText);
    }

    public void CompleteLevel(int nextLevelBuildIndex)
    {
        // Unlock the next level in the save file
        SaveManager.Instance.UnlockLevel(nextLevelBuildIndex);

        // Tell GameManager to load it
        GameManager.Instance.LoadScene(nextLevelBuildIndex);
    }
}