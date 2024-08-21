using UnityEngine;

public class EnemyAlertSystem : MonoBehaviour
{
    public float alertTime = 10f; // Time before alert status is shared with other enemies
    public bool isAlerting = false; // Indicates if the enemy is alerting

    private float alertTimer;

    void Start()
    {
        // Initialization code if needed
    }

    void Update()
    {
        if (isAlerting)
        {
            alertTimer -= Time.deltaTime;
            if (alertTimer <= 0)
            {
                // Reset alerting after some time if needed
                isAlerting = false;
            }
        }
    }

    public void AlertNearbyEnemies()
    {
        // Implement the logic to alert nearby enemies
        // For example:
        foreach (var enemy in FindObjectsOfType<EnemyAlertSystem>())
        {
            if (enemy != this)
            {
                enemy.isAlerting = true;
            }
        }

        // Start the alert timer
        alertTimer = alertTime;
        isAlerting = true;
    }
}
