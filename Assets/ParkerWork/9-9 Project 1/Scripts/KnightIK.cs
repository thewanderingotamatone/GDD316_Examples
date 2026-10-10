using UnityEngine;

[RequireComponent(typeof(Animator), typeof(KnightAim))]
public class KnightIK : MonoBehaviour
{
    public float ikWeight = 1f;
    public float handSpacing = 0.2f;  // gap between hands along the sword; 0 = same point
    public Vector3 handRotationOffset; // tune per rig so palms wrap the grip

    private Animator animator;
    private KnightAim aim;

    void Start()
    {
        animator = GetComponent<Animator>();
        aim = GetComponent<KnightAim>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        Vector3 dir = aim.AimDir;
        if (dir == Vector3.zero) return;

        // Both hands are centered on the SAME point, spread along the aim axis
        Vector3 center = aim.AimPoint;
        Vector3 rightPos = center + dir * (handSpacing * 0.5f);
        Vector3 leftPos  = center - dir * (handSpacing * 0.5f);

        Quaternion rot = Quaternion.LookRotation(dir, Vector3.back) * Quaternion.Euler(handRotationOffset);

        SetHand(AvatarIKGoal.RightHand, rightPos, rot);
        SetHand(AvatarIKGoal.LeftHand,  leftPos,  rot);
    }

    void SetHand(AvatarIKGoal goal, Vector3 pos, Quaternion rot)
    {
        animator.SetIKPositionWeight(goal, ikWeight);
        animator.SetIKRotationWeight(goal, ikWeight);
        animator.SetIKPosition(goal, pos);
        animator.SetIKRotation(goal, rot);
    }
}