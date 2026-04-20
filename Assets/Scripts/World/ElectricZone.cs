using UnityEngine;

public class ElectricZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.TryGetComponent(out ModuleAbilitySystem abilitySystem))
        {
            if (!abilitySystem.IsShockImmune())
            {
                if (col.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(new DamageData(
                        10f,
                        DamageType.Electric,
                        transform.position
                    ));
                }
            }
        }
    }
}