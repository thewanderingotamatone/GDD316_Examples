using UnityEngine;
using System.Collections.Generic;

public class SwordDamage : MonoBehaviour
{
    public int damage = 10;
    public float hitCooldown = 0.5f;     // time before the same enemy can be hit again

    [Header("Swing Speed")]
    public float minSwingSpeed = 3f;     // units/second the sword must be moving to register a hit (buggy cus sword can pass thru objects)
    public Transform speedPoint;         // measure speed at the blade tip instead of the sword's pivot (theres gotta be a better way to do this)
    public bool logSpeed = false;        // tick to see speed values in console menu (helped me "balance" this) (lowk could use this # for "damage" instead of swing speed)

    [SerializeField] private float currentSwingSpeed;   // visible in the Inspector while playing (erm waiter look up please)

    private Vector3 lastPos;
    private Dictionary<Health, float> lastHitTime = new Dictionary<Health, float>();

    Transform Tracked => speedPoint != null ? speedPoint : transform;

    void Start() => lastPos = Tracked.position;

    // Measure speed once per physics step (the same rate trigger events fire at for collision junk)
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

        // swingspeed doesn't meet critera: do nothing
        if (currentSwingSpeed < minSwingSpeed)
        {
            return;
        }

        if (lastHitTime.TryGetValue(health, out float last) && Time.time < last + hitCooldown)
            return;

        lastHitTime[health] = Time.time;

        health.TakeDamage(damage);
    }
}