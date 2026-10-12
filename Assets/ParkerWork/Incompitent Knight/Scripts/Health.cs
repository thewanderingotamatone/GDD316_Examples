using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public int maxHealth = 30;
    public int Current { get; private set; }

    public event Action<int, int> OnDamaged;   // (current HP, damage amount)
    public event Action OnDied;                // new: fired once at 0 HP

    private bool dead;

    void Awake() => Current = maxHealth;

    public void TakeDamage(int amount)
    {
        if (dead) return;   // new: ignore hits after death (since part of object pool)

        Current = Mathf.Max(0, Current - amount);   // new: HP never goes below 0
        Debug.Log($"{name} took {amount} damage. HP: {Current}/{maxHealth}");

        OnDamaged?.Invoke(Current, amount); // announce damage -----------------------

        if (Current == 0)
        {
            dead = true;
            OnDied?.Invoke();   // announce death ------------------------------------
        }
    }

    public void ResetHealth()
    {
        Current = maxHealth;
        dead = false;
    }
}