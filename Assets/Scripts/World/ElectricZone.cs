using UnityEngine;

public class ElectricZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D col)
    {
        // Check if it's the player and they have the ShockAbsorber equipped
        if (col.TryGetComponent(out PlayerEquipment equipment))
        {
            if (equipment.equippedModule != null && equipment.equippedModule.type == ModuleType.ShockAbsorber)
            {
                // They are passively immune! Stop running this code.
                return;
            }
        }

        // If no module is found, deal damage
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