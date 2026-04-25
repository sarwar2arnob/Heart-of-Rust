using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimationManager))]
[RequireComponent(typeof(PlayerEquipment))]
public class PlayerController : MonoBehaviour
{
    // ===== CORE COMPONENTS =====
    public Rigidbody2D rb { get; private set; }
    public PlayerEquipment equipment { get; private set; }
    public PlayerAnimationManager AnimManager { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }

    private InputHandler inputHandler;

    // ===== TOP-DOWN STATES =====
    public IdleState IdleState { get; private set; }
    public WalkState WalkState { get; private set; }
    public InteractState InteractState { get; private set; }
    public CraftingState CraftingState { get; private set; }
    public HurtState HurtState { get; private set; }
    public DashState DashState { get; private set; }

    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public Vector2 FacingDirection { get; private set; } = Vector2.right;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float lastDashTime { get; set; } = -100f;

    [Header("Interaction Settings")]
    public float interactionRadius = 1.5f;
    public LayerMask interactableLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        equipment = GetComponent<PlayerEquipment>();
        AnimManager = GetComponent<PlayerAnimationManager>();
        inputHandler = GetComponent<InputHandler>();

        rb.gravityScale = 0f;
        StateMachine = new PlayerStateMachine();

        IdleState = new IdleState(this, StateMachine, AnimManager);
        WalkState = new WalkState(this, StateMachine, AnimManager);
        InteractState = new InteractState(this, StateMachine, AnimManager);
        CraftingState = new CraftingState(this, StateMachine, AnimManager);
        HurtState = new HurtState(this, StateMachine, AnimManager);
        DashState = new DashState(this, StateMachine, AnimManager);
    }

    private void OnEnable()
    {
        if (inputHandler != null)
        {
            inputHandler.OnDash += HandleDash;
            inputHandler.OnInteract += HandleInteract;
            inputHandler.OnUseModule += HandleUseModule; // ✅
        }
    }

    private void OnDisable()
    {
        if (inputHandler != null)
        {
            inputHandler.OnDash -= HandleDash;
            inputHandler.OnInteract -= HandleInteract;
            inputHandler.OnUseModule -= HandleUseModule;
        }
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

    // ===== MOVEMENT LOGIC =====
    private void HandleMovement()
    {
        // 1. Block normal movement if interacting, crafting, hurt, or dashing
        if (StateMachine.CurrentState == InteractState ||
            StateMachine.CurrentState == CraftingState ||
            StateMachine.CurrentState == HurtState ||
            StateMachine.CurrentState == DashState)
        {
            // We don't want to kill velocity if we are mid-dash
            if (StateMachine.CurrentState != DashState)
            {
                rb.linearVelocity = Vector2.zero;
            }
            return;
        }

        // 2. Gating: Check if the chassis has the Leg Actuator unlocked
        if (!equipment.canMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // 3. Normal Omnidirectional Movement
        Vector2 moveInput = inputHandler.MoveDirection;
        rb.linearVelocity = moveInput * moveSpeed;

        // 4. RESTORED: Flip the sprite when moving left/right
        // (Make sure your Blend Tree uses the Right-facing clip for both the Left and Right nodes!)
        if (moveInput.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x), 1, 1);
        }

        if (moveInput.x != 0)
        {
            FacingDirection = new Vector2(moveInput.x, 0f).normalized;
        }
    }

    private void HandleDash()
    {
        // Gating & State Checks
        if (!equipment.canDash) return;
        if (Time.time < lastDashTime + dashCooldown) return;

        if (StateMachine.CurrentState == InteractState ||
            StateMachine.CurrentState == CraftingState ||
            StateMachine.CurrentState == HurtState ||
            StateMachine.CurrentState == DashState)
            return;

        // Prevent dashing in place
        if (inputHandler.MoveDirection.sqrMagnitude < 0.01f) return;

        StateMachine.ChangeState(DashState);
    }

    // ===== SINGLE INTERACTION FLOW =====
    // Fixed HandleInteract() in PlayerController.cs
    public void HandleInteract()
    {
        // 1. TOGGLE OFF: If currently interacting/crafting, cancel and return to idle
        if (StateMachine.CurrentState == InteractState || StateMachine.CurrentState == CraftingState)
        {
            if (StateMachine.CurrentState == CraftingState)
            {
                // Close the UI properly before changing state
                if (CraftingUI_Slots.Instance != null)
                    CraftingUI_Slots.Instance.Close();
            }

            StateMachine.ChangeState(IdleState);
            return;
        }

        // 2. TOGGLE ON: If free to move, check if something is nearby
        if (StateMachine.CurrentState == IdleState || StateMachine.CurrentState == WalkState)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, interactionRadius, interactableLayer);
            if (hit != null)
            {
                IInteractable interactableObject = hit.GetComponent<IInteractable>();
                if (interactableObject != null)
                {
                    interactableObject.Interact(this);
                }
            }
        }
    }

    public void HandleUseModule()
    {
        if (StateMachine.CurrentState == CraftingState ||
            StateMachine.CurrentState == HurtState)
            return;

        GetComponent<ModuleAbilitySystem>().UseModule();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}