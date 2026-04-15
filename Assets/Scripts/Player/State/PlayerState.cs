public abstract class PlayerState
{
    protected PlayerController player;
    protected PlayerStateMachine stateMachine;

    // We pass 'object anim' temporarily to match the 'null' we used in PlayerController.
    // Once you build your PlayerAnimationManager, you can change 'object' to 'PlayerAnimationManager'.
    public PlayerState(PlayerController player, PlayerStateMachine stateMachine, object anim)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    // Called once when entering the state
    public virtual void Enter() { }

    // Called every frame in Update()
    public virtual void LogicUpdate() { }

    // Called every physics frame in FixedUpdate() (if needed)
    public virtual void PhysicsUpdate() { }

    // Called once when exiting the state
    public virtual void Exit() { }
}