using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotFollow : MonoBehaviour
{
    public bool isFollowing;
    public RobotController robotController;
    public NavMeshAgent robotNavMesh;
    public Animator animator;
    public GameObject girl;
    public bool playerIsFarEnough;
    public float minRange;
    public float range;
    public float distance;
    public bool isInteracting;

    private void Awake()
    {
        robotNavMesh = GetComponent<NavMeshAgent>();
        robotNavMesh.enabled = false;
        isFollowing = false;
    }

    private void Update()
    {
        if (!robotController.isActive)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isFollowing)
                {
                    isFollowing = false;
                    robotNavMesh.enabled = false;
                    robotController.isFollowing = false;
                }
                else
                {
                    isFollowing = true;
                    robotNavMesh.enabled = true;
                    robotController.isFollowing = true;
                }
            }
            if (isFollowing)
            {
                Vector3 lookPosition = new Vector3(girl.transform.position.x, transform.position.y, girl.transform.position.z);
                distance = Vector3.Distance(girl.transform.position, transform.position);
                if (distance > range)
                {
                    playerIsFarEnough = true;
                }
                else if (distance < minRange)
                {
                    playerIsFarEnough = false;
                    animator.SetFloat("InputMagnitude", 0, 0.05f, Time.deltaTime);
                }
                if (playerIsFarEnough && !isInteracting && !robotController.isActive)
                {
                    robotNavMesh.SetDestination(girl.transform.position);
                    transform.LookAt(lookPosition);
                    animator.SetFloat("InputMagnitude", robotController.maxSpeed, 0.05f, Time.deltaTime);
                    Debug.Log("Animate Walking");
                }
            }
            else
            {
                animator.SetFloat("InputMagnitude", 0, 0.05f, Time.deltaTime);
            }
        }
    }
}
