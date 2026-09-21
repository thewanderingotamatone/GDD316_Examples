using UnityEngine;

public class KnightController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpHeight = 8f;
    public float gravity = -20f;

    private float moveX;
    private float verticalVelocity;

    private Animator animator;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleMovement();
        UpdateAnimations();
    }

    void HandleMovement()
    {
        moveX = Input.GetAxisRaw("Horizontal");

        // Horizontal movement
        float horizontal = moveX * speed;

        // Smooth gravity
        if (controller.isGrounded)
        {
            verticalVelocity = -2f; // small downward force to stay grounded

            if (Input.GetKeyDown(KeyCode.W))
            {
                // Smooth jump impulse
                verticalVelocity = Mathf.Sqrt(2f * jumpHeight * -gravity);
            }
        }
        else
        {
            // Apply gravity over time
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Combine movement
        Vector3 move = new Vector3(horizontal, verticalVelocity, 0f);

        controller.Move(move * Time.deltaTime);
    }

    void UpdateAnimations()
    {
        animator.SetFloat("MoveX", moveX);
        animator.SetBool("IsGrounded", controller.isGrounded);
        animator.SetFloat("VerticalVelocity", verticalVelocity);
    }
}
