public class InteractState : PlayerState
{
    public InteractState(PlayerController player, PlayerStateMachine stateMachine, object anim) : base(player, stateMachine, anim) { }
    // Logic for pushing boxes, picking up scrap, etc., will go here
}