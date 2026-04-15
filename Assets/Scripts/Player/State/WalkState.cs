public class WalkState : PlayerState
{
    public WalkState(PlayerController player, PlayerStateMachine stateMachine, object anim) : base(player, stateMachine, anim) { }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // If the player stops providing input, switch to IdleState
        if (InputHandler.Instance.MoveDirection.sqrMagnitude <= 0.01f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}