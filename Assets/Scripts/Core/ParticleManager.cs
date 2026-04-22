using System.Collections.Generic;
using UnityEngine;
using Singleton;

public enum ParticleType
{
    // Player
    PlayerDust,       // footstep puff
    PlayerDash,       // dash trail burst
    PlayerHurt,       // hit spark
    PlayerDeath,      // death explosion

    // Hazards
    ElectricSpark,
    FirePuff,
    GasCloud,
    IceShatter,

    // World
    ItemPickup,
    PressurePlatePress,
    RecipeUnlock,
    CraftSuccess
}

[System.Serializable]
public class ParticleEntry
{
    public ParticleType type;
    public ParticleSystem prefab;
}

public class ParticleManager : SingletonPersistent<ParticleManager>
{
    [Header("Particle Library")]
    [SerializeField] private List<ParticleEntry> particleEntries;

    private Dictionary<ParticleType, ParticleSystem> particleMap = new();

    protected override void Awake()
    {
        base.Awake();
        BuildDictionary();
    }

    private void BuildDictionary()
    {
        particleMap.Clear();
        if (particleEntries == null) return;
        foreach (var entry in particleEntries)
        {
            if (!particleMap.ContainsKey(entry.type))
                particleMap[entry.type] = entry.prefab; // 👈 was just 'entry'
            else
                Debug.LogWarning($"[ParticleManager] Duplicate ParticleType: {entry.type}");
        }
    }

    public void Play(ParticleType type, Vector2 position)
    {
        if (!particleMap.TryGetValue(type, out ParticleSystem prefab))
        {
            Debug.LogWarning($"[ParticleManager] Particle not found: {type}");
            return;
        }

        if (prefab == null)
        {
            Debug.LogWarning($"[ParticleManager] Prefab is null for: {type}");
            return;
        }

        ParticleSystem instance = Instantiate(prefab, position, Quaternion.identity);
        instance.Play();
        Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
    }

    public void Play(ParticleType type, Vector2 position, float scale)
    {
        if (!particleMap.TryGetValue(type, out ParticleSystem prefab))
        {
            Debug.LogWarning($"[ParticleManager] Particle not found: {type}");
            return;
        }

        ParticleSystem instance = Instantiate(prefab, position, Quaternion.identity);
        instance.transform.localScale = Vector3.one * scale;
        instance.Play();
        Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
    }
}