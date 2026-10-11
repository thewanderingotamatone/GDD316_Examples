using UnityEngine;

public class SwordFollow : MonoBehaviour
{
    public KnightAim aim;
    public Vector3 rotationOffset;   // replaces your lockedX / lockedZ values
    public Vector3 positionOffset;   // nudge the sword along its own handle

    void LateUpdate()
    {
        if (aim.AimDir == Vector3.zero) return;

        Quaternion rot = Quaternion.LookRotation(aim.AimDir, Vector3.back) * Quaternion.Euler(rotationOffset);
        transform.rotation = rot;
        transform.position = aim.AimPoint + rot * positionOffset;
    }
}