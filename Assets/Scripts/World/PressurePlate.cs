using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private bool isActivated = false;

    public void Activate()
    {
        if (isActivated) return;

        isActivated = true;

        Debug.Log("Pressure Plate Activated!");

        // TODO:
        // Trigger door, platform, etc.
    }
}