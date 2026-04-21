// Core/BootLoader.cs

using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    private void Awake()
    {
        // Force all persistent singletons to initialize in the right order
        // Accessing .Instance is enough — the singleton creates itself if missing

        var input = InputHandler.Instance;
        var game = GameManager.Instance;
        var audio = AudioManager.Instance;
        var save = SaveManager.Instance;
        var particle = ParticleManager.Instance;

        Debug.Log("[Boot] All singletons initialized.");
    }

    private void Start()
    {
        // Small delay so singletons finish their own Awake/Start before we leave
        SceneManager.LoadSceneAsync(1); // 1 = MainMenu
    }
}