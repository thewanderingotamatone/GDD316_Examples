using UnityEngine;

public class KnightIK : MonoBehaviour
{
    private Animator animator;
    public float ikWeight = 1f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;

        // ayo the mouse here (checks mouse input)
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 target = hit.point;

            // lefty loosey (left hand)
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, target);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.LookRotation(target - animator.GetIKPosition(AvatarIKGoal.LeftHand)));

            // righty tighty (right hand)
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeight);
            animator.SetIKPosition(AvatarIKGoal.RightHand, target);
            animator.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.LookRotation(target - animator.GetIKPosition(AvatarIKGoal.RightHand)));
        }
    }
}
