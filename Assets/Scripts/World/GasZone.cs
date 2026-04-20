using UnityEngine;

public class GasZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D col)
    {
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