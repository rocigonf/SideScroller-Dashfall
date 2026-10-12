using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float torqueAmount = 1f;
    InputAction moveAction;
    Vector2 moveInput;
    Rigidbody2D characterBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        characterBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerTorque();
    }

    // Applies torque to the player based on the horizontal input from the Move action
    void PlayerTorque()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        if (moveInput.x < 0)
        {
            characterBody.AddTorque(torqueAmount);
        } 
        else if (moveInput.x > 0)
        {
            characterBody.AddTorque(-torqueAmount);
        }
    }
}
