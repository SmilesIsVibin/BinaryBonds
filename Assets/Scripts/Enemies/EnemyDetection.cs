using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public Transform player;
    public LayerMask viewMask; // Mask for objects that block the view
    public LayerMask transparentObjectsMask; // Mask for objects that are transparent or partially block view (e.g., glass, fences)
    public float viewAngle = 45f;
    public float viewDistance = 10f;
    public float detectionTime = 2f;
    public float suspicionDecayRate = 1f;

    private float detectionTimer = 0f;
    public Vector3 lastKnownPlayerPosition;
    public float SuspicionLevel { get; private set; } = 0f;
    public bool IsPlayerDetected => SuspicionLevel >= detectionTime;

    private EnemyAIController enemyAI;

    void Start()
    {
        enemyAI = GetComponent<EnemyAIController>();
    }

    public void HandleDetection()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (Vector3.Angle(transform.forward, directionToPlayer) < viewAngle / 2f && distanceToPlayer <= viewDistance)
        {
            RaycastHit hit;

            // Check if there's a clear line of sight to the player
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, viewDistance, viewMask))
            {
                // Handle transparent objects
                if (((1 << hit.collider.gameObject.layer) & transparentObjectsMask) != 0)
                {
                    IncreaseSuspicionLevel();
                }
                else
                {
                    DecreaseSuspicionLevel();
                }
            }
            else
            {
                IncreaseSuspicionLevel();
            }
        }
        else
        {
            DecreaseSuspicionLevel();
        }
    }

    public void IncreaseSuspicionLevel()
    {
        SuspicionLevel += Time.deltaTime;

        if (IsPlayerDetected)
        {
            enemyAI.SetState(EnemyAIController.EnemyState.Alerted);
            lastKnownPlayerPosition = player.position;
        }
        else
        {
            enemyAI.SetState(EnemyAIController.EnemyState.Suspicious);
        }
    }

    private void DecreaseSuspicionLevel()
    {
        if (SuspicionLevel > 0)
        {
            SuspicionLevel -= suspicionDecayRate * Time.deltaTime;

            if (SuspicionLevel <= 0)
            {
                SuspicionLevel = 0;
                if (enemyAI.currentState == EnemyAIController.EnemyState.Suspicious)
                {
                    enemyAI.SetState(EnemyAIController.EnemyState.Returning);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (player == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Gizmos.color = Color.green;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * viewDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward * viewDistance;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (leftBoundary + rightBoundary) / 2);
    }
}
