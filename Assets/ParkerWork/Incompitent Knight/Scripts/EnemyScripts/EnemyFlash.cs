using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Health))]
public class EnemyFlash : MonoBehaviour
{
    public Renderer rend;                  // the enemy's Mesh Renderer
    public Color flashColor = Color.red;
    public float fadeInTime = 0.05f;       // original -> red (quick)
    public float fadeOutTime = 0.25f;      // red -> original (slower)

    private Health health;
    private Material mat;
    private string colorProperty;
    private Color originalColor;

    void Awake()
    {
        health = GetComponent<Health>();

        mat = rend.material;   // per-enemy copy
        colorProperty = mat.HasProperty("_BaseColor") ? "_BaseColor" : "_Color";
        originalColor = mat.GetColor(colorProperty);
    }

    void OnEnable()  => health.OnDamaged += HandleDamaged;
    void OnDisable() => health.OnDamaged -= HandleDamaged;

    void HandleDamaged(int current, int amount)
    {
        StopAllCoroutines();   // restart the flash if hit again mid-flash
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        // Start from whatever color we're at right now, so a re-hit mid-fade doesn't pop
        Color startColor = mat.GetColor(colorProperty);

        // Fade in: current -> red
        for (float t = 0f; t < fadeInTime; t += Time.deltaTime)
        {
            mat.SetColor(colorProperty, Color.Lerp(startColor, flashColor, t / fadeInTime));
            yield return null;
        }
        mat.SetColor(colorProperty, flashColor);

        // Fade out: red -> original
        for (float t = 0f; t < fadeOutTime; t += Time.deltaTime)
        {
            mat.SetColor(colorProperty, Color.Lerp(flashColor, originalColor, t / fadeOutTime));
            yield return null;
        }
        mat.SetColor(colorProperty, originalColor);
    }
}