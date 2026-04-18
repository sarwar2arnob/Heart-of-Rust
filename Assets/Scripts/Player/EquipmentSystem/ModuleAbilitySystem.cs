using System.Collections;
using UnityEngine;

public class ModuleAbilitySystem : MonoBehaviour
{
    private PlayerEquipment equipment;
    private PlayerController playerController;

    private bool isShockImmune = false;

    void Awake()
    {
        equipment = GetComponent<PlayerEquipment>();
        playerController = GetComponent<PlayerController>();
    }

    public void UseModule()
    {
        var module = equipment.equippedModule;

        if (module == null)
        {
            Debug.Log("No module equipped!");
            return;
        }

        switch (module.type)
        {
            case ModuleType.IceBreaker:
                UseIceBreaker();
                break;

            case ModuleType.FireExtinguisher:
                UseFireExtinguisher();
                break;

            case ModuleType.ShockAbsorber:
                UseShockAbsorber();
                break;

            case ModuleType.HeavyCore:
                UseHeavyCore();
                break;
        }
    }

    void UseIceBreaker()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position + transform.right, 1f);

        if (hit != null)
        {
            IceBlock ice = hit.GetComponent<IceBlock>();
            if (ice != null)
                ice.Break();
        }
    }

    void UseFireExtinguisher()
    {
        float range = 2f;
        float radius = 1.2f;

        Vector2 center = (Vector2)transform.position + playerController.FacingDirection * range * 0.5f;

        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius);

        foreach (var hit in hits)
        {
            FireZone fire = hit.GetComponent<FireZone>();
            if (fire != null)
            {
                fire.Extinguish();
            }
        }

        Debug.DrawLine(transform.position, center, Color.red, 1f);
    }

    void UseShockAbsorber()
    {
        if (!isShockImmune)
            StartCoroutine(ShockRoutine());
    }

    IEnumerator ShockRoutine()
    {
        isShockImmune = true;
        Debug.Log("Shock immunity ON");

        yield return new WaitForSeconds(3f);

        isShockImmune = false;
        Debug.Log("Shock immunity OFF");
    }

    public bool IsShockImmune()
    {
        return isShockImmune;
    }

    void UseHeavyCore()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 2f);

        foreach (var hit in hits)
        {
            PressurePlate plate = hit.GetComponent<PressurePlate>();
            if (plate != null)
                plate.Activate();
        }
    }
}