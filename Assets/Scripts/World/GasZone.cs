using UnityEngine;

public class GasZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D col)
    {
        // 1. Check if the player has the FilterCore equipped passively
        if (col.TryGetComponent(out PlayerEquipment equipment))
        {
            if (equipment.equippedModule != null && equipment.equippedModule.type == ModuleType.FilterCore)
            {
                // Player is breathing easy. Do not apply damage!
                return;
            }
        }

        // 2. Apply damage if no filter is equipped
        if (col.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(new DamageData(
                5f,
                DamageType.Gas,
                transform.position
            ));
        }
    }
}