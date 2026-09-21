using UnityEngine;

public class SwordRotateToCursor : MonoBehaviour
{
    public Transform grip;          // grip parented to right hand
    public Transform chest;         // spine 2 chest bone
    public Transform leftHandIK;    // ik target for left hand

    public float lockedX = 90f;
    public float lockedZ = 270f;
    public float upwardOffset = 0f;

    void LateUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 target = hit.point;

            Vector3 dir = target - chest.position;
            dir.z = 0f;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            Vector3 rot = grip.localEulerAngles;
            rot.x = lockedX;
            rot.y = angle;
            rot.z = lockedZ;

            grip.localEulerAngles = rot;

            grip.localPosition = new Vector3(
                grip.localPosition.x,
                upwardOffset,
                grip.localPosition.z
            );

            // make left hand follow the grip empty
            // future me make sure to have the grip empty do the math to stay between the hands
            if (leftHandIK != null)
            {
                leftHandIK.position = grip.position;
                leftHandIK.rotation = grip.rotation;
            }
        }
    }
}
