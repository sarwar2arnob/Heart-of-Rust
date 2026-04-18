using UnityEngine;

public class ElectricZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.TryGetComponent(out ModuleAbilitySystem abilitySystem))
        {
            if (!abilitySystem.IsShockImmune())
            {
                Debug.Log("⚡ Taking electric damage!");
            }
        }
    }
}