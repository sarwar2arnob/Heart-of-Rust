using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerEquipment))] // Automatically ensures the equipment script is attached
public class PlayerController : MonoBehaviour
{
    // Changed to public properties with private setters so states can access them
    public Rigidbody2D rb { get; private set; }
    public PlayerEquipment equipment { get; private set; }

    public PlayerStateMachine StateMachine { get; private set; }

    // ===== TOP-DOWN STATES =====
    public IdleState IdleState { get; private set; }
    public WalkState WalkState { get; private set; }
    public InteractState InteractState { get; private set; }
    public CraftingState CraftingState { get; private set; }
    public HurtState HurtState { get; private set; }
    public DashState DashState { get; private set; } // New Dash State

    [Header("Movement Settings")]
    public float moveSpeed = 6f;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float lastDashTime { get; set; } = -100f; // Tracks when we last dashed

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        equipment = GetComponent<PlayerEquipment>();

        // Ensure zero gravity for top-down
        rb.gravityScale = 0f;

        StateMachine = new PlayerStateMachine();

        IdleState = new IdleState(this, StateMachine, null);
        WalkState = new WalkState(this, StateMachine, null);
        InteractState = new InteractState(this, StateMachine, null);
        CraftingState = new CraftingState(this, StateMachine, null);
        HurtState = new HurtState(this, StateMachine, null);
        DashState = new DashState(this, StateMachine, null); // Initialize Dash
    }

    private void OnEnable()
    {
        if (InputHandler.Instance != null)
            InputHandler.Instance.OnDash += HandleDash;
    }

    private void OnDisable()
    {
        if (InputHandler.Instance != null)
            InputHandler.Instance.OnDash -= HandleDash;
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.Update();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    // ===== MOVEMENT & DASH LOGIC =====

    private void HandleMovement()
    {
        // 1. Block normal movement if interacting, crafting, hurt, or dashing
        //if (StateMachine.CurrentState == InteractState ||
        //    StateMachine.CurrentState == CraftingState ||
        //    StateMachine.CurrentState == HurtState ||
        //    StateMachine.CurrentState == DashState)
        //{
        //    // We don't want to kill velocity if we are dashing!
        //    if (StateMachine.CurrentState != DashState)
        //    {
        //        rb.linearVelocity = Vector2.zero;
        //    }
        //    return;
        //}

        //// 2. Gating: Check if the chassis has the Leg Actuator unlocked
        //if (!equipment.canMove)
        //{
        //    rb.linearVelocity = Vector2.zero;
        //    return;
        //}

        // 3. Normal Omnidirectional Movement
        Vector2 moveInput = InputHandler.Instance.MoveDirection;
        rb.linearVelocity = moveInput * moveSpeed;

        if (moveInput.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x), 1, 1);
        }
    }

    private void HandleDash()
    {
        // 1. Gating: Ensure the Jump Servo / Dash Unit is unlocked
        //if (!equipment.canDash) return;

        // 2. Cooldown check
        if (Time.time < lastDashTime + dashCooldown) return;

        // 3. Block dashing if stuck in a menu, interacting, or taking damage
        if (StateMachine.CurrentState == InteractState ||
            StateMachine.CurrentState == CraftingState ||
            StateMachine.CurrentState == HurtState ||
            StateMachine.CurrentState == DashState)
            return;

        // 4. Prevent dashing completely in place (must have a direction)
        if (InputHandler.Instance.MoveDirection.sqrMagnitude < 0.01f) return;

        StateMachine.ChangeState(DashState);
    }

    // ===== INTERACTION =====

    public void TriggerInteraction(IInteractable interactableObject)
    {
        if (interactableObject == null) return;
        StateMachine.ChangeState(InteractState);
        interactableObject.Interact(this);
    }

    public void OpenToolbox()
    {
        StateMachine.ChangeState(CraftingState);
    }

    public void CloseToolbox()
    {
        if (StateMachine.CurrentState == CraftingState)
        {
            StateMachine.ChangeState(IdleState);
        }
    }

    public void ReleaseInteraction()
    {
        if (StateMachine.CurrentState == InteractState)
        {
            StateMachine.ChangeState(IdleState);
        }
    }
}