using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Example in C# for Unity
public class RobotRunner : MonoBehaviour
{
    public Animator animator;
    public float baseSpeed = 10f;                    // The default running speed
    public float currentSpeed;                       // The player's current speed
    public float sideSpeed = 5f;                     // Speed for moving left/right
    public float smallObstacleSpeedPenalty = 2f;     // Penalty for hitting a small obstacle
    public float largeObstacleSpeedPenalty = 5f;     // Penalty for hitting a large obstacle
    public float speedRecoveryRate = 1f;             // How fast the player recovers speed
    public float invincibleTimeAfterHit = 2f;        // Time after hit where player can't be slowed again
    private float invincibleTimer = 0f;              // Tracks invincibility duration
    private bool isInvincible = false;               // Flag for invincibility

    public int maxAllowedHits = 3;                   // Max consecutive hits before game over
    private int currentHits = 0;                     // Tracks how many hits have occurred in quick succession

    public float hitCooldownTime = 2f;               // Time after which the hit counter resets if no hits
    private float hitCooldownTimer = 0f;             // Timer for tracking cooldown between hits

    void Start()
    {
        currentSpeed = baseSpeed;
        animator.SetFloat("InputMagnitude", 13f);
    }

    void Update()
    {
        // Forward movement (could be constant)
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        // Sideways movement (left and right)
        float moveHorizontal = Input.GetAxis("Horizontal"); // Use arrow keys or A/D for side movement
        transform.Translate(Vector3.right * moveHorizontal * sideSpeed * Time.deltaTime);

        // Speed recovery logic
        if (currentSpeed < baseSpeed && !isInvincible)
        {
            currentSpeed += speedRecoveryRate * Time.deltaTime;
        }

        // Handle invincibility after hit
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                isInvincible = false;
            }
        }

        // Handle cooldown for resetting hits if enough time has passed without a hit
        if (currentHits > 0)
        {
            hitCooldownTimer -= Time.deltaTime;
            if (hitCooldownTimer <= 0)
            {
                currentHits = 0;  // Reset the hit counter if no hits have occurred for the cooldown period
                Debug.Log("Hit counter reset due to no consecutive hits.");
            }
        }
    }

    // When hit by an obstacle
    public void HitObstacle(string obstacleType)
    {
        if (isInvincible) return; // Ignore hits when invincible

        // Handle different obstacle types
        if (obstacleType == "Breakable")
        {
            // No penalty for breakable obstacles
            return;
        }
        else if (obstacleType == "Small")
        {
            currentSpeed -= smallObstacleSpeedPenalty;
        }
        else if (obstacleType == "Large")
        {
            currentSpeed -= largeObstacleSpeedPenalty;
        }

        // Update the hit counter and cooldown timer
        currentHits += 1;
        hitCooldownTimer = hitCooldownTime; // Reset the cooldown timer for consecutive hits

        // Check if the player has exceeded the allowed number of hits
        if (currentHits >= maxAllowedHits)
        {
            // Trigger the game over or enemy catching up logic
            GameOver();
        }

        // Activate invincibility for a short period
        isInvincible = true;
        invincibleTimer = invincibleTimeAfterHit;

        Debug.Log("Player hit! Current hits: " + currentHits);
    }

    void GameOver()
    {
        // Handle the game over logic (enemies catch up)
        Debug.Log("Game Over! You were caught!");
    }

}

