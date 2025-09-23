using UnityEngine;
using UnityEngine.InputSystem;

public class CarInputHandler : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private Vector2 moveInput;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetMoveInput()
    {
        return moveInput;
    }
}
