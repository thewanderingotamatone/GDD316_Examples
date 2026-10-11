using UnityEngine;
using System.Collections.Generic;

public class SwordDamage : MonoBehaviour
{
    public int damage = 10;
    public float hitCooldown = 0.5f;     // time before the same enemy can be hit again

    [Header("Swing Speed")]
    public float minSwingSpeed = 3f;     // units/second the sword must be moving to register a hit
    public Transform speedPoint;         // optional: measure speed at the blade tip instead of the sword's pivot
    public bool logSpeed = false;        // tick to see speed values in the Console

    [SerializeField] private float currentSwingSpeed;   // visible in the Inspector while playing

    private Vector3 lastPos;
    private Dictionary<Health, float> lastHitTime = new Dictionary<Health, float>();

    Transform Tracked => speedPoint != null ? speedPoint : transform;

    void Start() => lastPos = Tracked.position;

    // Measure speed once per physics step, the same rate trigger events fire at
    void FixedUpdate()
    {
        Vector3 pos = Tracked.position;
        currentSwingSpeed = (pos - lastPos).magnitude / Time.fixedDeltaTime;
        lastPos = pos;
    }

    void OnTriggerEnter(Collider other) => TryHit(other);
    void OnTriggerStay(Collider other)  => TryHit(other);

    void TryHit(Collider other)
    {
        Health health = other.GetComponentInParent<Health>();
        if (health == null) return;

        // Too slow? Skip. This runs BEFORE the cooldown is stamped,
        // so a slow touch never "uses up" the cooldown.
        if (currentSwingSpeed < minSwingSpeed)
        {
            if (logSpeed) Debug.Log($"Too slow on {health.name}: {currentSwingSpeed:F1} < {minSwingSpeed}");
            return;
        }

        if (lastHitTime.TryGetValue(health, out float last) && Time.time < last + hitCooldown)
            return;

        lastHitTime[health] = Time.time;

        Debug.Log($"Sword hit {health.name} at speed {currentSwingSpeed:F1}");
        health.TakeDamage(damage);
    }
}