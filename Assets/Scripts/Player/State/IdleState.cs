public class IdleState : PlayerState
{
    public IdleState(PlayerController player, PlayerStateMachine stateMachine, object anim) : base(player, stateMachine, anim) { }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // If the player presses a directional key, switch to WalkState
        if (InputHandler.Instance.MoveDirection.sqrMagnitude > 0.01f)
        {
            stateMachine.ChangeState(player.WalkState);
        }
    }
}