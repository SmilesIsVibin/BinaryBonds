using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Example obstacle script
public class RunnerObstacle : MonoBehaviour
{
    public string obstacleType; // Can be "Breakable", "Small", or "Large"

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RobotRunner player = other.GetComponent<RobotRunner>();
            if (player != null)
            {
                player.HitObstacle(obstacleType);
                if (obstacleType == "Breakable")
                {
                    Destroy(gameObject); // Breakable obstacles are destroyed on collision
                }
            }
        }
    }
}

