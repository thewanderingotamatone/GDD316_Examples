using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyDeath : MonoBehaviour
{
    public float destroyDelay = 0.3f;   // lets the red flash finish before the enemy vanishes

    private Health health;

    void Awake() => health = GetComponent<Health>();

    void OnEnable()  => health.OnDied += HandleDied;
    void OnDisable() => health.OnDied -= HandleDied;

    void HandleDied()
    {
        Destroy(gameObject, destroyDelay);
    }
}