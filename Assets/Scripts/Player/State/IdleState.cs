public class IdleState : PlayerState
{
    // Change 'object anim' to 'PlayerAnimationManager animManager'
    public IdleState(PlayerController player, PlayerStateMachine stateMachine, PlayerAnimationManager animManager)
        : base(player, stateMachine, animManager) { }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        animManager.UpdateMovementAnimation(player.GetComponent<InputHandler>().MoveDirection);

        if (player.GetComponent<InputHandler>().MoveDirection.sqrMagnitude > 0.01f)
        {
            stateMachine.ChangeState(player.WalkState);
        }
    }
}