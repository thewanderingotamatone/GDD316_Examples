using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeedForward = 5f;
    public float walkSpeedBackward = 3f;
    public float crouchSpeed = 2f;

    public float blendSpeed = 8f;

    private float currentSpeed;
    private float moveSpeedParam;
    private float sneakingParam;

    private bool isCrouching = false;

    private CharacterController controller;
    private Animator animator;

    void Start()
    {
        // CharacterController put on empty
        controller = GetComponent<CharacterController>();

        // Animator is on the child model
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleCrouchInput();
        HandleMovement();
        UpdateBlendTreeParameters();
    }

    void HandleCrouchInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) // funny story i thought there was a problem with my code for ages but my animations were just in the wrong spots. siigh.
            isCrouching = true;

        if (Input.GetKeyUp(KeyCode.LeftControl))
            isCrouching = false;
    }

    void HandleMovement()
    {
        float vertical = Input.GetAxisRaw("Vertical"); // W&S type shit movement

        if (vertical > 0)
            currentSpeed = isCrouching ? crouchSpeed : walkSpeedForward;
        else if (vertical < 0)
            currentSpeed = isCrouching ? crouchSpeed : walkSpeedBackward;
        else
            currentSpeed = 0;

        Vector3 move = transform.forward * vertical * currentSpeed;
        controller.Move(move * Time.deltaTime);
    }

    void UpdateBlendTreeParameters()
    {
        float normalizedSpeed = Mathf.Clamp01(currentSpeed / walkSpeedForward);
        moveSpeedParam = Mathf.MoveTowards(moveSpeedParam, normalizedSpeed, Time.deltaTime * blendSpeed);

        float targetSneak = isCrouching ? 1f : 0f;
        sneakingParam = Mathf.MoveTowards(sneakingParam, targetSneak, Time.deltaTime * blendSpeed);

        animator.SetFloat("MoveSpeed", moveSpeedParam);
        animator.SetFloat("Sneaking", sneakingParam);
    }
}
