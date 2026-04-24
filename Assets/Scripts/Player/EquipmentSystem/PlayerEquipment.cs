using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private float lastSwapTime = 0f;
    private float swapCooldown = 0.25f;
    private CraftingSystem craftingSystem;
    [Header("Debug / Testing")]
    public bool unlockAllOnStart = false;

    private InputHandler inputHandler;


    [Header("Permanent Parts")]
    public bool canMove { get; private set; }
    public bool canDash { get; private set; }
    public bool canInteract { get; private set; }

    [Header("Swappable Modules")]
    public ModuleData equippedModule { get; private set; }

    private List<ModuleData> unlockedModules = new List<ModuleData>();
    private int currentModuleIndex = -1;

    private void Start()
    {
        if (unlockAllOnStart)
        {
            canMove = true;
            canInteract = true;
            canDash = true;

            Debug.LogWarning("[DEBUG] All abilities unlocked!");
        }
        else
        {
            // 🔥 REAL GAME START STATE
            canMove = false;        // unlocked in Level 1
            canInteract = true;     // always available
            canDash = false;        // unlocked via JumpServo
        }

        DebugCurrentState();

        craftingSystem = FindAnyObjectByType<CraftingSystem>();
        if (craftingSystem != null)
            craftingSystem.OnCraftSuccess += ApplyCraftResult;
        else
            Debug.LogError("[PlayerEquipment] CraftingSystem not found!");

        inputHandler = GetComponent<InputHandler>();
    }

    private void OnEnable()
    {
        if (inputHandler != null)
            inputHandler.OnSwapModule += CycleModule;
    }

    private void OnDisable()
    {
        if (inputHandler != null)
            inputHandler.OnSwapModule -= CycleModule;
    }

    // =========================
    // 🔥 ENTRY FROM CRAFTING
    // =========================
    public void ApplyCraftResult(CraftResult result)
    {
        if (result.type == ResultType.Part)
            UnlockPart(result.part);
        else if (result.type == ResultType.Module)
            UnlockModule(result.module);
    }

    // =========================
    // 🔧 PART UNLOCKING
    // =========================
    private void UnlockPart(PartType part)
    {
        switch (part)
        {
            case PartType.LegActuator:
                if (!canMove)
                {
                    canMove = true;
                    Debug.Log("Unlocked: Leg Actuator → Movement enabled");
                }
                break;

            case PartType.ManipulatorArm:
                if (!canInteract)
                {
                    canInteract = true;
                    Debug.Log("Unlocked: Manipulator Arm → Interaction enabled");
                }
                break;

            case PartType.JumpServo:
                if (!canDash)
                {
                    canDash = true;
                    Debug.Log("🔥 Unlocked: Jump Servo → DASH ENABLED!");
                }
                break;
        }

        DebugCurrentState();
    }

    // =========================
    // 🔧 MODULE SYSTEM
    // =========================
    private void UnlockModule(ModuleData newModule)
    {
        if (newModule == null) return;

        if (!unlockedModules.Contains(newModule))
        {
            unlockedModules.Add(newModule);
            Debug.Log($"Module Unlocked: {newModule.type}");

            // Auto-equip first module
            if (equippedModule == null)
            {
                EquipModule(newModule);
            }
        }
    }

    private void EquipModule(ModuleData module)
    {
        if (module == null) return;

        equippedModule = module;
        currentModuleIndex = unlockedModules.IndexOf(module);

        Debug.Log($"Module Equipped: {module.type}");
    }

    private void CycleModule(float direction)
    {
        // 🛑 Prevent rapid double-trigger
        if (Time.time - lastSwapTime < swapCooldown)
            return;

        lastSwapTime = Time.time;

        if (unlockedModules.Count <= 1) return;

        if (direction > 0)
        {
            currentModuleIndex = (currentModuleIndex + 1) % unlockedModules.Count;
        }
        else if (direction < 0)
        {
            currentModuleIndex--;
            if (currentModuleIndex < 0)
                currentModuleIndex = unlockedModules.Count - 1;
        }

        EquipModule(unlockedModules[currentModuleIndex]);
    }

    private void OnDestroy()
    {
        if (craftingSystem != null)
            craftingSystem.OnCraftSuccess -= ApplyCraftResult;
    }
    // =========================
    // 🧪 DEBUG
    // =========================
    private void DebugCurrentState()
    {
        Debug.Log($"[Equipment] Move:{canMove} | Interact:{canInteract} | Dash:{canDash}");
    }
}