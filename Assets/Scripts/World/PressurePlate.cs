using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private bool isActivated = false;

    void OnTriggerStay2D(Collider2D col)
    {
        var module = col.GetComponent<ModuleSystem>();

        if (module != null && module.HasModule(ModuleType.HeavyCore))
        {
            Activate();
        }
    }

    void Activate()
    {
        if (isActivated) return;

        isActivated = true;
        Debug.Log("Pressure Plate Activated");

        // TODO: open door / trigger mechanism
    }
}