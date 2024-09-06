using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    public enum EnemyState { Idle, Patrolling, Suspicious, Alerted, Chasing, Returning }
    public EnemyState currentState;

    private NavMeshAgent agent;
    private EnemyDetection detection;
    private EnemyChase chase;
    private EnemyAlertSystem alertSystem;

    public Transform[] patrolPoints;
    public float idleTimeMin = 2f;
    public float idleTimeMax = 5f;
    public float closeEnoughDistance = 1.5f; // Distance to consider close to a patrol point

    private int currentPatrolIndex;
    private float idleTimer;
    private bool isIdle;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        detection = GetComponent<EnemyDetection>();
        chase = GetComponent<EnemyChase>();
        alertSystem = GetComponent<EnemyAlertSystem>();

        if (patrolPoints.Length > 1)
        {
            currentState = EnemyState.Patrolling;
            ChooseNextPatrolPoint();
        }
        else
        {
            currentState = EnemyState.Idle;
        }
    }

    void Update()
    {
        detection.HandleDetection();

        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Patrolling:
                Patrol();
                break;
            case EnemyState.Suspicious:
                Suspicious();
                break;
            case EnemyState.Alerted:
                Alerted();
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Returning:
                ReturnToPatrol();
                break;
        }
    }

    void Idle()
    {
        if (!isIdle)
        {
            isIdle = true;
            idleTimer = Random.Range(idleTimeMin, idleTimeMax);
        }

        idleTimer -= Time.deltaTime;

        if (idleTimer <= 0)
        {
            isIdle = false;
            SetState(EnemyState.Patrolling);
        }
    }

    void Patrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            if (Random.Range(0f, 1f) < 0.3f)
            {
                SetState(EnemyState.Idle);
                return;
            }

            ChooseNextPatrolPoint();
        }
    }

    void ChooseNextPatrolPoint()
    {
        if (patrolPoints.Length > 1)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    void Suspicious()
    {
        agent.isStopped = true;
        detection.HandleDetection(); // Adjusted to use detection properly

        if (detection.IsPlayerDetected)
        {
            SetState(EnemyState.Alerted);
        }
        else if (detection.SuspicionLevel <= 0)
        {
            SetState(EnemyState.Returning);
        }
    }

    void Alerted()
    {
        alertSystem.AlertNearbyEnemies();
        agent.SetDestination(detection.lastKnownPlayerPosition);
        SetState(EnemyState.Chasing);
    }

    void ChasePlayer()
    {
        if (detection.IsPlayerDetected)
        {
            agent.SetDestination(detection.lastKnownPlayerPosition);
        }
        else
        {
            SetState(EnemyState.Returning);
        }
    }

    void ReturnToPatrol()
    {
        if (!agent.pathPending && agent.remainingDistance <= closeEnoughDistance)
        {
            if (alertSystem.isAlerting) // Check if alerting is active
            {
                return; // Stay in returning state if alerting
            }

            SetState(EnemyState.Patrolling);
        }
        else
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    public void SetState(EnemyState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case EnemyState.Idle:
                agent.isStopped = true;
                break;
            case EnemyState.Patrolling:
                agent.isStopped = false;
                ChooseNextPatrolPoint();
                break;
            case EnemyState.Suspicious:
                agent.isStopped = true;
                break;
            case EnemyState.Alerted:
                agent.isStopped = false;
                break;
            case EnemyState.Chasing:
                agent.isStopped = false;
                break;
            case EnemyState.Returning:
                agent.isStopped = false;
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
                break;
        }
    }
}
