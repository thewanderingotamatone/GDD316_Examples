using UnityEngine;

public class KnightAim : MonoBehaviour
{
    public Transform chest;          // spine 2 / chest bone
    public float minReach = 0.3f;    // closest the hands get to the chest
    public float maxReach = 0.7f;    // arm length limit (tune to your rig)

    public Vector3 AimPoint { get; private set; }  // the shared hand point
    public Vector3 AimDir { get; private set; }    // chest -> cursor, flat on XY

    void Update()
    {
        // Mathematical wall plane at the knight's depth (no collider needed)
        Plane wall = new Plane(Vector3.back, chest.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!wall.Raycast(ray, out float enter)) return;

        Vector3 mouseWorld = ray.GetPoint(enter);
        Vector3 toMouse = mouseWorld - chest.position;
        toMouse.z = 0f;

        if (toMouse.sqrMagnitude < 0.0001f) return;

        AimDir = toMouse.normalized;
        float dist = Mathf.Clamp(toMouse.magnitude, minReach, maxReach);
        AimPoint = chest.position + AimDir * dist;
    }
}