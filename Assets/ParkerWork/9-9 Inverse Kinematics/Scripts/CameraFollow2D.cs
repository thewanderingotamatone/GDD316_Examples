using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;        // Knight
    public float smoothY = 4f;      // How fast camera catches up vertically
    public float yInfluence = 0.25f; // How much the knight's Y affects the camera

    private float baseY;            // Camera's resting height

    void Start()
    {
        baseY = transform.position.y;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;

        // Compute damped target Y
        float targetY = Mathf.Lerp(
            baseY,                     // resting height
            target.position.y,         // knight's height
            yInfluence                 // partial influence
        );

        // Smoothly move toward targetY
        pos.y = Mathf.Lerp(pos.y, targetY, smoothY * Time.deltaTime);

        transform.position = pos;
    }
}
