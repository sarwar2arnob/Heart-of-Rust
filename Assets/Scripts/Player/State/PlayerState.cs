public abstract class PlayerState
{
    protected PlayerController player;
    protected PlayerStateMachine stateMachine;
    protected PlayerAnimationManager animManager; // <-- Updated type

    public PlayerState(PlayerController player, PlayerStateMachine stateMachine, PlayerAnimationManager animManager)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animManager = animManager; // <-- Assign it
    }

    public virtual void Enter() { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void Exit() { }
}