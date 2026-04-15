using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [Header("Permanent Parts")]
    public bool canMove { get; private set; }
    public bool canDash { get; private set; }
    public bool canInteract { get; private set; }

    [Header("Swappable Modules")]
    public ModuleData equippedModule { get; private set; }

    private List<ModuleData> unlockedModules = new List<ModuleData>();
    private int currentModuleIndex = -1;

    private void OnEnable()
    {
        // Subscribe to the input event for swapping modules
        if (InputHandler.Instance != null)
            InputHandler.Instance.OnSwapModule += CycleModule;
    }

    private void OnDisable()
    {
        // Always unsubscribe to prevent memory leaks
        if (InputHandler.Instance != null)
            InputHandler.Instance.OnSwapModule -= CycleModule;
    }

    // --- Progression Integration ---

    // Call this method from whatever script listens to CraftingSystem.OnCraftSuccess
    public void ApplyCraftResult(CraftResult result)
    {
        if (result.type == ResultType.Part)
            UnlockPart(result.part);
        else if (result.type == ResultType.Module)
            UnlockModule(result.module);
    }

    private void UnlockPart(PartType part)
    {
        switch (part)
        {
            case PartType.LegActuator:
                canMove = true;
                Debug.Log("Unlocked: Leg Actuator! Movement enabled.");
                break;
            case PartType.JumpServo:
                canDash = true;
                Debug.Log("Unlocked: Jump Servo! Mobility enhanced.");
                break;
            case PartType.ManipulatorArm:
                canInteract = true;
                Debug.Log("Unlocked: Manipulator Arm! Interaction enabled.");
                break;
        }
    }

    private void UnlockModule(ModuleData newModule)
    {
        if (!unlockedModules.Contains(newModule))
        {
            unlockedModules.Add(newModule);
            Debug.Log($"Module Unlocked: {newModule.type}");

            // Auto-equip if it's the first module we've built
            if (equippedModule == null)
            {
                EquipModule(newModule);
            }
        }
    }

    private void EquipModule(ModuleData module)
    {
        equippedModule = module;
        currentModuleIndex = unlockedModules.IndexOf(module);
        Debug.Log($"Module Equipped: {module.type}");
    }

    private void CycleModule(float direction)
    {
        if (unlockedModules.Count <= 1) return;

        // Cycle right
        if (direction > 0)
        {
            currentModuleIndex = (currentModuleIndex + 1) % unlockedModules.Count;
        }
        // Cycle left
        else if (direction < 0)
        {
            currentModuleIndex--;
            if (currentModuleIndex < 0) currentModuleIndex = unlockedModules.Count - 1;
        }

        EquipModule(unlockedModules[currentModuleIndex]);
    }
}