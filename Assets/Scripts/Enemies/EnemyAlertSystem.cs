using UnityEngine;

public class EnemyAlertSystem : MonoBehaviour
{
    public float alertTime = 10f; // Time before alert status is shared with other enemies
    public bool isAlerting = false; // Indicates if the enemy is alerting

    private float alertTimer;

    void Update()
    {
        if (isAlerting)
        {
            alertTimer -= Time.deltaTime;
            if (alertTimer <= 0)
            {
                isAlerting = false;
            }
        }
    }

    public void AlertNearbyEnemies()
    {
        foreach (var enemy in FindObjectsOfType<EnemyAlertSystem>())
        {
            if (enemy != this)
            {
                enemy.isAlerting = true;
            }
        }
        alertTimer = alertTime;
        isAlerting = true;
    }
}
