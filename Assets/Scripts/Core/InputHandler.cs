using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Singleton;

public class InputHandler : SingletonPersistent<InputHandler>
{
    private PlayerInput playerInput;
    private InputAction moveInput;
    public Vector2 MoveDirection { get; private set; }
    public event Action OnInteract;
    public event Action OnDash;
    public event Action OnUseModule;
    public event Action OnCraftingOpen;
    public event Action<float> OnSwapModule;
    public event Action OnPause;
   
    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError($"[InputHandler] PlayerInput component is missing on {gameObject.name}");
        }
    }

    private void Start()
    {
        if (playerInput != null)
        {
            moveInput = playerInput.actions.FindAction("Move");
            if (moveInput == null)
            {
                Debug.LogError($"[InputHandler] Move action is missing in the Input Action Asset!");
            }
        }
    }

    private void Update()
    {
        MoveAction();
    }

    private void MoveAction()
    {
        if (moveInput != null)
        {
            // Normalized prevents the player from moving faster diagonally
            MoveDirection = moveInput.ReadValue<Vector2>().normalized;
        }
    }

    // Connect these methods to your Unity Events on the PlayerInput component in the Inspector
    public void InteractAction(InputAction.CallbackContext context)
    {
        if (context.performed) OnInteract?.Invoke();
    }

    public void CraftingAction(InputAction.CallbackContext context)
    {
        if (context.performed) OnCraftingOpen?.Invoke();
    }

    public void SwapModuleAction(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        float value = context.ReadValue<float>();

        if (Mathf.Abs(value) < 0.5f) return;

        OnSwapModule?.Invoke(Mathf.Sign(value));
    }

    public void DashAction(InputAction.CallbackContext context)
    {
        if (context.performed) OnDash?.Invoke();
    }

    public void PauseAction(InputAction.CallbackContext context)
    {
        if (context.performed) OnPause?.Invoke();
        Debug.Log("Pause action performed");
    }

    public void UseModuleAction(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnUseModule?.Invoke();
    }

    public void SwitchActionMap(string actionMapName)
    {
        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap(actionMapName);
        }
    }
}