using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPush : MonoBehaviour
{
    [SerializeField]
    private float pushForce;
    [SerializeField]
    private float massLimit;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private RobotController controller;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (controller.isActive)
        {
            Rigidbody objectRB = hit.collider.attachedRigidbody;

            if (objectRB != null)
            {
                Vector3 forceDir = hit.gameObject.transform.position - transform.position;
                forceDir.y = 0;
                forceDir.Normalize();

                objectRB.AddForceAtPosition(forceDir * pushForce, transform.position, ForceMode.Impulse);
                if (objectRB.mass >= massLimit)
                {
                    Debug.Log("Push Animation");
                    animator.SetBool("isPushing", true);
                }
                else
                {
                    animator.SetBool("isPushing", false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(controller.isActive)
        {
            if (animator.GetBool("isPushing"))
            {
                animator.SetBool("isPushing", false);
            }
        }
    }
}
