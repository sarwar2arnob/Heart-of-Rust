using UnityEngine;
using Singleton;

[System.Serializable]
public class SaveData
{
    public int highestUnlockedLevel = 1; // Default to level 1 unlocked
    // You can add settings here later (e.g., masterVolume, musicVolume)
}

public class SaveManager : SingletonPersistent<SaveManager>
{
    private const string SAVE_KEY = "HeartOfRust_SaveData";
    public SaveData CurrentData { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        LoadGame();
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            CurrentData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            CurrentData = new SaveData();
        }
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(CurrentData);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save(); // Crucial for WebGL
    }

    public void UnlockLevel(int levelIndex)
    {
        if (levelIndex > CurrentData.highestUnlockedLevel)
        {
            CurrentData.highestUnlockedLevel = levelIndex;
            SaveGame();
        }
    }
}