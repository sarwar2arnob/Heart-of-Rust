using UnityEngine;

public class DashState : PlayerState
{
    private float dashStartTime;
    private Vector2 dashDirection;

    public DashState(PlayerController player, PlayerStateMachine stateMachine, object anim) : base(player, stateMachine, anim) { }

    public override void Enter()
    {
        base.Enter();

        dashStartTime = Time.time;

        // Lock in the direction the player was holding when they pressed Dash
        dashDirection = InputHandler.Instance.MoveDirection.normalized;

        // Apply the immediate burst of speed
        player.rb.linearVelocity = dashDirection * player.dashSpeed;

        // Optional: Trigger a particle effect or animation here
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Check if the dash duration has finished
        if (Time.time >= dashStartTime + player.dashDuration)
        {
            // Transition back to Idle or Walk based on current input
            if (InputHandler.Instance.MoveDirection.sqrMagnitude > 0.01f)
            {
                stateMachine.ChangeState(player.WalkState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        // Stop the high velocity so the player doesn't slide
        player.rb.linearVelocity = Vector2.zero;

        // Record the time we finished the dash to start the cooldown timer
        player.lastDashTime = Time.time;
    }
}