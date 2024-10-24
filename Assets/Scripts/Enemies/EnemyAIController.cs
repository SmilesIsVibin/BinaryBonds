using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    public enum EnemyState { Idle, Patrolling, Suspicious, Alerted, Chasing, Returning }
    public EnemyState currentState;
    private NavMeshAgent agent;
    private EnemyDetection detection;
    private EnemyAlertSystem alertSystem;
    public Animator animator;

    public Transform[] patrolPoints;
    public float idleTimeMin = 2f;
    public float idleTimeMax = 5f;
    public float closeEnoughDistance = 1.5f; // Distance to consider close to a patrol point

    private int currentPatrolIndex;
    private float idleTimer;
    private bool isIdle;
    public bool alerted;

    public float chaseTime = 5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        detection = GetComponent<EnemyDetection>();
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
            animator.SetBool("isIdling", true);
            animator.SetBool("isPatrolling", false);
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
                Debug.Log("Enemy is Idling");
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
            animator.SetBool("isPatrolling", true);
        }
    }

    void Suspicious()
    {
        agent.isStopped = true;
        detection.HandleDetection(); // Adjusted to use detection properly

        if (detection.IsPlayerDetected && !alerted)
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
        Debug.Log("Alerted");
        alerted = true;
        animator.SetBool("isAlerted", true);
        alertSystem.AlertNearbyEnemies();
        animator.SetBool("isIdling", false);
        animator.SetBool("isPatrolling", false);
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

    public void InitiateChase(){
        Debug.Log("Chasing");
        animator.SetBool("isAlerted", false);
        animator.SetBool("isChasing", true);
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
            animator.SetBool("isChasing", false);
        }
        else
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            animator.SetBool("isIdling", false);
            animator.SetBool("isPatrolling", true);
            animator.SetBool("isChasing", false);
            alerted = false;
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

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Player_Elara")){
            PlayerManager.Instance.PlayerGameOver();
        }
    }
}
