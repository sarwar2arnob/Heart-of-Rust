using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [Header("Debug / Testing")]
    public bool unlockAllOnStart = true; // Uncheck this later when building the real game!

    [Header("Permanent Parts")]
    public bool canMove;
    public bool canDash;
    public bool canInteract;

    [Header("Swappable Modules")]
    public ModuleData equippedModule { get; private set; }

    private List<ModuleData> unlockedModules = new List<ModuleData>();
    private int currentModuleIndex = -1;

    private void Start()
    {
        // TEMPORARY TESTING LOGIC
        if (unlockAllOnStart)
        {
            canMove = true;
            canDash = true;
            canInteract = true;
            Debug.LogWarning("[DEBUG] All chassis abilities temporarily unlocked for testing!");
        }
    }

    private void OnEnable()
    {
        if (InputHandler.Instance != null)
            InputHandler.Instance.OnSwapModule += CycleModule;
    }

    private void OnDisable()
    {
        if (InputHandler.Instance != null)
            InputHandler.Instance.OnSwapModule -= CycleModule;
    }

    // --- Progression Integration ---
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

        if (direction > 0)
        {
            currentModuleIndex = (currentModuleIndex + 1) % unlockedModules.Count;
        }
        else if (direction < 0)
        {
            currentModuleIndex--;
            if (currentModuleIndex < 0) currentModuleIndex = unlockedModules.Count - 1;
        }

        EquipModule(unlockedModules[currentModuleIndex]);
    }
}