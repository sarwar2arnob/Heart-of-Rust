using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public string clueText;

    void Start()
    {
        FindObjectOfType<ClueManager>().ShowClue(clueText);
    }

    public void CompleteLevel()
    {
        // load next scene
    }
}