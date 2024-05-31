using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotPickup : MonoBehaviour
{
    [SerializeField] private Transform objectTransform;
    public RobotController robotController;
    bool canPickup;
    [SerializeField] private GameObject pickedObject;
    bool hasPickupObject;
    [SerializeField] private Animator animator;

    void Start()
    {
        canPickup = false;
        hasPickupObject = false;
    }

    private void Update()
    {
        if (robotController.isActive)
        {
            if (canPickup)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    animator.SetBool("isPickingUp", true);
                }
            }

            if (hasPickupObject)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    animator.SetBool("isDropping", true);
                }
            }
        }
    }

    public void Pickup()
    {
        animator.SetLayerWeight(1, 1);
        pickedObject.GetComponent<Rigidbody>().isKinematic = true;
        pickedObject.GetComponent<Collider>().isTrigger = true;
        pickedObject.GetComponent<NavMeshObstacle>().enabled = false;
        pickedObject.transform.position = objectTransform.position;
        pickedObject.transform.parent = objectTransform;
        hasPickupObject = true;
        animator.SetBool("isPickingUp", false);
    }

    public void Drop()
    {
        animator.SetLayerWeight(1, 0);
        pickedObject.GetComponent<Rigidbody>().isKinematic = false;
        pickedObject.GetComponent<Collider>().isTrigger = false;
        pickedObject.GetComponent<NavMeshObstacle>().enabled = true;
        pickedObject.transform.parent = null;
        pickedObject = null;
        canPickup = false;
        hasPickupObject = false;
        animator.SetBool("isDropping", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp") && hasPickupObject == false)
        {
            canPickup = true;
            pickedObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canPickup = false;
    }
}
