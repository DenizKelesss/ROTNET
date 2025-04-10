using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSmoothTime = 0.1f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public Transform cam;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private CharacterController controller;
    private PlayerInputActions inputActions;
    private Vector2 inputMove;
    private Vector3 velocity;
    private bool isGrounded;
    private float turnSmoothVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => inputMove = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => inputMove = Vector2.zero;
        inputActions.Player.Jump.performed += ctx => Jump();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Update()
    {
        HandleMovement();
        HandleGravity();
    }

    void HandleMovement()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Vector3 direction = new Vector3(inputMove.x, 0f, inputMove.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
    }

    void HandleGravity()
    {
        // Check if grounded, and reset the velocity.y when touching the ground
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;  // Small negative value to keep grounded

        // Apply gravity over time
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Debug: Visualize the ground check with a ray
        Debug.DrawRay(groundCheck.position, Vector3.down * groundDistance, Color.red);
    }

    void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);  // Calculate the jump force
        }
    }
}
