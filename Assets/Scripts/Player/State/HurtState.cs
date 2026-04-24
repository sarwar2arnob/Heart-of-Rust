using UnityEngine;

public class HurtState : PlayerState
{
    private float hurtDuration = 0.4f; // how long the stun lasts
    private float enterTime;

    public HurtState(PlayerController player, PlayerStateMachine stateMachine, PlayerAnimationManager animManager)
        : base(player, stateMachine, animManager) { }

    public override void Enter()
    {
        base.Enter();
        enterTime = Time.time;

        // knock the player back slightly
        Vector2 knockback = -player.FacingDirection * 3f;
        player.rb.linearVelocity = knockback;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= enterTime + hurtDuration)
        {
            if (player.GetComponent<InputHandler>().MoveDirection.sqrMagnitude > 0.01f)
                stateMachine.ChangeState(player.WalkState);
            else
                stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.linearVelocity = Vector2.zero;
    }
}