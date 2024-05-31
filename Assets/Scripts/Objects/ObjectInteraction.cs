using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public float pushForce;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody _rigg = hit.collider.attachedRigidbody;
        if (_rigg != null)
        {
            Debug.Log("Collided");

            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            _rigg.AddForceAtPosition(forceDirection * pushForce, transform.position, ForceMode.Impulse);
        }
    }
}
