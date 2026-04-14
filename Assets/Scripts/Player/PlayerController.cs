using UnityEngine;

[RequireComponent(typeof(PlayerState))]
public class PlayerController : MonoBehaviour
{
    private PlayerState state;
    private Rigidbody2D rb;

    private Vector2 moveInput;
    private bool jumpPressed;

    [SerializeField] private InputHandler input;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 6f;

    void Awake()
    {
        state = GetComponent<PlayerState>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        input.OnMove += HandleMove;
        input.OnJump += HandleJump;
    }

    void OnDisable()
    {
        input.OnMove -= HandleMove;
        input.OnJump -= HandleJump;
    }

    void FixedUpdate()
    {
        if (state.canMove)
            Move();

        if (state.canJump && jumpPressed)
        {
            Jump();
            jumpPressed = false;
        }
    }

    public void HandleMove(Vector2 input)
    {
        moveInput = input;
    }

    public void HandleJump()
    {
        jumpPressed = true;
    }

    public void Move()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
}