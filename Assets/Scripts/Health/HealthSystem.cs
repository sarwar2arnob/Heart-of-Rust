using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    public float MaxHealth => maxHealth;
    public float CurrentHealth { get; private set; }

    public bool IsDead => CurrentHealth <= 0;

    [Header("Damage Settings")]
    [SerializeField] private float invincibilityDuration = 0.5f;

    private float lastDamageTime = -999f;

    // Optional references
    private PlayerController player;
    private PlayerAnimationManager anim;

    // Events
    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;
    public event Action<DamageData> OnDamaged;

    private void Awake()
    {
        CurrentHealth = maxHealth;

        player = GetComponent<PlayerController>();
        anim = GetComponent<PlayerAnimationManager>();
    }

    public void TakeDamage(DamageData damage)
    {
        if (IsDead) return;

        // 🛡 Invincibility frames
        if (Time.time < lastDamageTime + invincibilityDuration)
            return;

        lastDamageTime = Time.time;

        // Apply damage
        CurrentHealth -= damage.amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        Debug.Log($"Took {damage.amount} {damage.type} damage");

        // 🔔 Events
        OnDamaged?.Invoke(damage);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        // 🎬 Feedback
        if (anim != null)
        {
            anim.FlashColor(Color.red, 0.1f);
        }

        // 🎮 Player state reaction
        if (player != null && !IsDead)
        {
            player.StateMachine.ChangeState(player.HurtState);
        }

        // 💀 Death
        if (IsDead)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (IsDead) return;

        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    private void Die()
    {
        Debug.Log("Entity Died");
        OnDeath?.Invoke();

        if (player != null)
            player.enabled = false;

        // 👇 Tell GameManager the player died
        GameManager.Instance?.TriggerGameOver();
    }
}