using UnityEngine;
public class PlayerState : MonoBehaviour
{
    public bool canMove;
    public bool canJump;
    public bool canInteract;

    public ModuleData equippedModule;

    public void UnlockPart(PartType part)
    {
        switch (part)
        {
            case PartType.LegActuator:
                canMove = true;
                break;
            case PartType.JumpServo:
                canJump = true;
                break;
            case PartType.ManipulatorArm:
                canInteract = true;
                break;
        }
    }

    public void EquipModule(ModuleData module)
    {
        equippedModule = module;
    }
}