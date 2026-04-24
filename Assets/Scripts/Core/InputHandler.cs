using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction moveInput;

    public Vector2 MoveDirection { get; private set; }

    public event Action OnInteract;
    public event Action OnDash;
    public event Action OnUseModule;
    public event Action<float> OnSwapModule;
    public event Action OnPause;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        moveInput = playerInput.actions.FindAction("Move");

        if (moveInput == null)
        {
            Debug.LogError("[InputHandler] Move action missing!");
        }
    }

    private void Update()
    {
        if (moveInput != null)
        {
            MoveDirection = moveInput.ReadValue<Vector2>().normalized;
        }
    }

    public void InteractAction(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnInteract?.Invoke();
    }

    public void CraftingAction(InputAction.CallbackContext context)
    {

    }

    public void SwapModuleAction(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        float value = context.ReadValue<float>();

        if (Mathf.Abs(value) < 0.5f)
            return;

        OnSwapModule?.Invoke(Mathf.Sign(value));
    }

    public void DashAction(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnDash?.Invoke();
    }

    public void PauseAction(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnPause?.Invoke();
    }

    public void UseModuleAction(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnUseModule?.Invoke();
    }

    public void SwitchActionMap(string actionMapName)
    {
        playerInput.SwitchCurrentActionMap(actionMapName);
    }
}