using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Health))]
public class EnemyDeath : MonoBehaviour
{
    public float returnDelay = 0.3f;   // lets red flash finish

    // spawner assigns this.
    // if null, fall back to Destroy.
    [NonSerialized] public Action<GameObject> returnToPool;

    private Health health;

    void Awake() => health = GetComponent<Health>();

    void OnEnable()  => health.OnDied += HandleDied;
    void OnDisable() => health.OnDied -= HandleDied;

    void HandleDied() => StartCoroutine(ReturnAfterDelay());

    IEnumerator ReturnAfterDelay()
    {
        yield return new WaitForSeconds(returnDelay);

        if (returnToPool != null) returnToPool(gameObject);
        else Destroy(gameObject);
    }
}