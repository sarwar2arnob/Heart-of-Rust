using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputHandler : MonoBehaviour
{
    private GameInput input;

    // EVENTS (this is the key)
    public event Action<Vector2> OnMove;
    public event Action OnJump;

    void Awake()
    {
        input = new GameInput();
    }

    void OnEnable()
    {
        input.Enable();

        input.Player.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
        input.Player.Move.canceled += ctx => OnMove?.Invoke(Vector2.zero);

        input.Player.Jump.performed += ctx => OnJump?.Invoke();
    }

    void OnDisable()
    {
        input.Disable();
    }
}