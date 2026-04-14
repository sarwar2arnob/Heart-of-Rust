using UnityEngine;

public class GasZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        var module = col.GetComponent<ModuleSystem>();

        if (!module.HasModule(ModuleType.FilterCore))
        {
            // damage or block
        }
    }
}