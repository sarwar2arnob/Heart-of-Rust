using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    [Header("Visual Effects")]
    [SerializeField] private Image fillImage;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float smoothSpeed = 0.2f;

    private Color originalColor;
    private Coroutine smoothRoutine;

    private void Start()
    {
        if (healthSystem == null)
        {
            Debug.LogError("HealthSystem not assigned!");
            return;
        }

        // Cache original color
        if (fillImage != null)
            originalColor = fillImage.color;

        // Subscribe
        healthSystem.OnHealthChanged += UpdateUI;
        healthSystem.OnDamaged += HandleDamage;
        healthSystem.OnDeath += HandleDeath;

        // Initialize
        UpdateUI(healthSystem.CurrentHealth, healthSystem.MaxHealth);
    }

    private void OnDestroy()
    {
        if (healthSystem == null) return;

        healthSystem.OnHealthChanged -= UpdateUI;
        healthSystem.OnDamaged -= HandleDamage;
        healthSystem.OnDeath -= HandleDeath;
    }

    private void UpdateUI(float current, float max)
    {
        float normalized = current / max;

        // Smooth transition
        if (smoothRoutine != null)
            StopCoroutine(smoothRoutine);

        smoothRoutine = StartCoroutine(SmoothHealth(normalized));

        // Text update
        if (healthText != null)
        {
            healthText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
        }
    }

    private IEnumerator SmoothHealth(float target)
    {
        float start = healthSlider.value;
        float time = 0f;

        while (time < smoothSpeed)
        {
            time += Time.deltaTime;
            healthSlider.value = Mathf.Lerp(start, target, time / smoothSpeed);
            yield return null;
        }

        healthSlider.value = target;
    }

    private void HandleDamage(DamageData damage)
    {
        if (fillImage != null)
        {
            StopCoroutine(nameof(Flash));
            StartCoroutine(Flash());
        }
    }

    private IEnumerator Flash()
    {
        fillImage.color = damageColor;
        yield return new WaitForSeconds(0.1f);
        fillImage.color = originalColor;
    }

    private void HandleDeath()
    {
        Debug.Log("Show Game Over UI");

        // Future:
        // - Enable Game Over Panel
        // - Fade screen
        // - Disable input
    }
}