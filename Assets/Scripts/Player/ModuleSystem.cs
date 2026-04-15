using UnityEngine;

public class ModuleSystem : MonoBehaviour
{
    private PlayerEquipment equipment; // Changed from PlayerState

    void Awake()
    {
        // Changed to find the new PlayerEquipment component
        equipment = GetComponent<PlayerEquipment>();
    }

    public bool HasModule(ModuleType type)
    {
        // Assuming your ModuleData has a 'type' property defined somewhere
        return equipment.equippedModule != null &&
               equipment.equippedModule.type == type;
    }
}