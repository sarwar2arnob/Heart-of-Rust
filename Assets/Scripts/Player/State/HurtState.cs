public class HurtState : PlayerState
{
    public HurtState(PlayerController player, PlayerStateMachine stateMachine, PlayerAnimationManager animManager)
        : base(player, stateMachine, animManager) { }

    // Logic for taking damage from gas/electricity will go here
}