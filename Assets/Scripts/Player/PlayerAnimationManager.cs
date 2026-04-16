using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimationManager : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    // Hashing strings for performance
    private readonly int moveXHash = Animator.StringToHash("MoveX");
    private readonly int moveYHash = Animator.StringToHash("MoveY");
    private readonly int speedHash = Animator.StringToHash("Speed");
    private readonly int dashTriggerHash = Animator.StringToHash("Dash");
    private readonly int interactTriggerHash = Animator.StringToHash("Interact");

    // Track the last facing direction so we don't snap to default when stopping
    private Vector2 lastFacingDirection = Vector2.down;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateMovementAnimation(Vector2 moveInput)
    {
        // If we are moving, update the facing direction
        if (moveInput.sqrMagnitude > 0.01f)
        {
            // STRICT 4-WAY SNAPPING
            // Compare the absolute values to find the dominant axis of movement
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                // Moving mostly horizontally
                lastFacingDirection = new Vector2(Mathf.Sign(moveInput.x), 0);
            }
            else
            {
                // Moving mostly vertically
                lastFacingDirection = new Vector2(0, Mathf.Sign(moveInput.y));
            }
        }

        // Feed the locked 4-way direction into the Blend Tree
        anim.SetFloat(moveXHash, lastFacingDirection.x);
        anim.SetFloat(moveYHash, lastFacingDirection.y);

        // Feed the raw magnitude to control the Idle -> Walk transition
        anim.SetFloat(speedHash, moveInput.sqrMagnitude);
    }

    public void TriggerDash()
    {
        anim.SetTrigger(dashTriggerHash);
    }

    public void TriggerInteract()
    {
        anim.SetTrigger(interactTriggerHash);
    }

    // Optional: useful if your chassis gets damaged and needs to flash red
    public void FlashColor(Color color, float duration)
    {
        // Implementation for a quick sprite flash
    }
}