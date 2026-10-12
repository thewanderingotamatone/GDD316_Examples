using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Health))]
public class EnemyChase : MonoBehaviour
{
    public Transform target;            // the knight (auto-found if left empty)
    public float speed = 2f;            // keep this below the knight's speed
    public float stopDistance = 1.2f;   // stops this far away instead of walking into the knight
    public float gravity = -20f;

    private CharacterController controller;
    private Health health;
    private float verticalVelocity;
    private bool alive;                 // missing line issue resolved (dumb dumb idiot dumb)

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        health = GetComponent<Health>();
    }

    void Start()
    {
        if (target == null)
        {
            KnightController knight = FindObjectOfType<KnightController>();
            if (knight != null) target = knight.transform;
        }
    }

    void OnEnable()
    {
        health.OnDied += HandleDied;
        alive = true;
        verticalVelocity = 0f;   // fresh start for a reused enemy
    }

    void OnDisable() => health.OnDied -= HandleDied;

    void HandleDied() => alive = false;

    void Update()
    {
        if (!alive || target == null) return;

        // horizontal direction toward the knight (X only, since the game is on a 2D plane)
        float dx = target.position.x - transform.position.x;
        float moveX = 0f;

        if (Mathf.Abs(dx) > stopDistance)
            moveX = Mathf.Sign(dx) * speed;

        // gravity (same approach as KnightController)
        if (controller.isGrounded) verticalVelocity = -2f;
        else verticalVelocity += gravity * Time.deltaTime;

        controller.Move(new Vector3(moveX, verticalVelocity, 0f) * Time.deltaTime);

        // face the knight (if i can get a good model soon siiiiiiigh)
        if (moveX != 0f)
            transform.rotation = Quaternion.Euler(0f, moveX > 0f ? 90f : -90f, 0f);
    }
}