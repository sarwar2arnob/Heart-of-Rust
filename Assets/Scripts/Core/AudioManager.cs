using System.Collections.Generic;
using UnityEngine;
using Singleton;

public enum SoundType
{
    // Player
    PlayerFootstep,
    PlayerDash,
    PlayerHurt,
    PlayerDeath,
    PlayerInteract,

    // Combat / Hazards
    ElectricZap,
    FireCrackle,
    GasHiss,
    IceBreak,

    // Crafting
    CraftSuccess,
    CraftFail,

    // UI
    ButtonClick,
    PauseOpen,
    PauseClose,

    // World
    PressurePlateClick,
    ItemPickup,
    RecipeUnlock
}

[System.Serializable]
public class SoundEntry
{
    public SoundType type;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.8f, 1.2f)] public float pitchVariance = 1f; // randomizes pitch slightly
}

public class AudioManager : SingletonPersistent<AudioManager>
{
    [Header("Sound Library")]
    [SerializeField] private List<SoundEntry> soundEntries;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    private Dictionary<SoundType, SoundEntry> soundMap = new();

    protected override void Awake()
    {
        base.Awake();
        BuildDictionary();
    }

    private void BuildDictionary()
    {
        soundMap.Clear();
        foreach (var entry in soundEntries)
        {
            if (!soundMap.ContainsKey(entry.type))
                soundMap[entry.type] = entry;
            else
                Debug.LogWarning($"[AudioManager] Duplicate SoundType: {entry.type}");
        }
    }

    public void Play(SoundType type)
    {
        if (!soundMap.TryGetValue(type, out SoundEntry entry))
        {
            Debug.LogWarning($"[AudioManager] Sound not found: {type}");
            return;
        }

        if (entry.clip == null)
        {
            Debug.LogWarning($"[AudioManager] Clip is null for: {type}");
            return;
        }

        sfxSource.pitch = Random.Range(1f / entry.pitchVariance, entry.pitchVariance);
        sfxSource.PlayOneShot(entry.clip, entry.volume);
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic() => musicSource.Stop();

    public void SetMusicVolume(float volume) => musicSource.volume = Mathf.Clamp01(volume);
    public void SetSFXVolume(float volume) => sfxSource.volume = Mathf.Clamp01(volume);
}