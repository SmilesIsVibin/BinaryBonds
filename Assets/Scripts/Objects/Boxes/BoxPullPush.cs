using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPullPush : MonoBehaviour
{
    private FixedJoint fixedJoint;
    public Rigidbody playerRB;

    public void AttachToPlayer()
    {
        fixedJoint.connectedBody = playerRB;
    }

    public void DetachFromPlayer()
    {
        fixedJoint.connectedBody = null;
    }

    void OnJointBreak(float breakForce)
    {
        // Handle what happens if the joint breaks (e.g., the box falls)
        DetachFromPlayer();
    }

    public void CreateFixedJoint(){
        fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = null; // Start without a connected body
        fixedJoint.breakForce = Mathf.Infinity;
        fixedJoint.breakTorque = Mathf.Infinity;
        fixedJoint.enableCollision = true;
    }

    public void RemoveFixedJoint(){
        fixedJoint = null;
        Destroy(gameObject.GetComponent<FixedJoint>());
    }
}
