using UnityEngine;


public class PlayerEquipment : MonoBehaviour
{
    // Note: If you want to check these in the Inspector, remove 'private set' 
    // and add [SerializeField] or just make them public without getters/setters.
    public bool canMove { get; private set; }
    public bool canDash { get; private set; }
    public bool canInteract { get; private set; }

    public ModuleData equippedModule { get; private set; }

    // --- ADD THIS FOR TESTING ---
    private void Start()
    {
        // Force unlock everything so you can test movement and dashing!
        UnlockPart(PartType.LegActuator);
        UnlockPart(PartType.JumpServo);
        UnlockPart(PartType.ManipulatorArm);
    }
    // ----------------------------

    public void UnlockPart(PartType part)
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

    public void EquipModule(ModuleData module)
    {
        equippedModule = module;
        Debug.Log("Module Equipped!");
    }
}