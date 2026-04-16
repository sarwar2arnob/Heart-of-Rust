public class InteractState : PlayerState
{
    public InteractState(PlayerController player, PlayerStateMachine stateMachine, PlayerAnimationManager animManager)
        : base(player, stateMachine, animManager) { }

    // Logic for pushing boxes, picking up scrap, etc., will go here
}