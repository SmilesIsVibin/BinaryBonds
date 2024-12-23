using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Idle, Patrol, Suspicion, Chase, Return }
    [SerializeField] private EnemyState currentState;

    // NavMesh and movement
    private NavMeshAgent agent;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    // Patrol-related variables
    public Transform[] patrolPoints;
    public int patrolIndex;
    private bool patrolForward = true;
    public float idleDurationMin = 1f, idleDurationMax = 3f;
    private float idleTimer;
    public bool isStationary = false;

    // Detection & suspicion
    public Transform player;
    public float frontFOVAngle = 90f;
    public float backFOVAngle = 45f;
    public float fovRange = 10f;
    public float backFOVRange = 3f;
    public float verticalFOV = 45f;

    public float idleRotTimer = 5f;

    public float idleRotCount;
    public LayerMask viewMask;
    public float suspicionThreshold = 5f;
    [SerializeField]private float suspicionLevel;
    public float closeDetectionRange = 1.5f;

    // Chase state variables
    public float detectionPauseTime = 0.5f;
    //private bool isAlerted;
    public float disinterestTimerMax = 5f;
    private float disinterestTimer;

    public Animator animator;

    // Return state variables
    private Vector3 lastKnownPlayerPosition;
    public GameObject suspicionIndicator;
    
    // Kill trigger
    public BoxCollider killZone;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Idle;
        patrolIndex = 0;
        idleTimer = Random.Range(idleDurationMin, idleDurationMax);
        killZone.enabled = false; // Disable kill zone at the start
        disinterestTimer = disinterestTimerMax;
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                animator.SetBool("isIdling", true);
                animator.SetBool("isPatrolling", false);
                HandleIdle();
                break;
            case EnemyState.Patrol:
                animator.SetBool("isIdling", false);
                animator.SetBool("isPatrolling", true);
                HandlePatrol();
                break;
            case EnemyState.Suspicion:
                HandleSuspicion();
                break;
            case EnemyState.Chase:
                animator.SetBool("isIdling", false);
                animator.SetBool("isPatrolling", false);
                animator.SetBool("isChasing", true);
                HandleChase();
                break;
            case EnemyState.Return:
                animator.SetBool("isPatrolling", true);
                animator.SetBool("isChasing", false);
                HandleReturn();
                break;
        }

        // Always check for player detection
        if (currentState != EnemyState.Chase)
        {
            CheckForPlayer();
        }
    }

    // Handle the Idle state
    void HandleIdle()
    {
        if (isStationary) // Rotate in place to simulate guard looking around
        {
            idleRotCount += Time.deltaTime;
            if(idleRotCount >= idleRotTimer){
                transform.Rotate(0, Random.Range(-45f, 45f) * Time.deltaTime, 0);
                idleRotCount = 0f;
            }
        }

        if(patrolPoints.Length > 1){
            idleTimer -= Time.deltaTime;

            if (idleTimer <= 0)
            {
                idleTimer = Random.Range(idleDurationMin, idleDurationMax);
                isStationary = false;
                currentState = EnemyState.Patrol;
            }
        }
    }

    // Handle the Patrol state
    void HandlePatrol()
    {
        agent.speed = patrolSpeed;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (Random.Range(1, 10f) < 3f) // 30% chance to idle
            {
                Debug.Log("Enemy is now idling, change from patrol");
                currentState = EnemyState.Idle;
                idleTimer = Random.Range(idleDurationMin, idleDurationMax);
                isStationary = true;
            }
            else
            {
                SetNextPatrolPoint();
            }
        }
    }

    // Set the next patrol point
    void SetNextPatrolPoint()
    {
        if (patrolForward)
        {
            patrolIndex++;
            if (patrolIndex >= patrolPoints.Length)
            {
                patrolIndex = patrolPoints.Length - 1;
                patrolForward = false;
            }
        }
        else
        {
            patrolIndex--;
            if (patrolIndex < 0)
            {
                patrolIndex = 0;
                patrolForward = true;
            }
        }
        agent.SetDestination(patrolPoints[patrolIndex].position);
    }

    // Handle the Suspicion state
    void HandleSuspicion()
    {
        suspicionIndicator.SetActive(true);
        suspicionLevel += Time.deltaTime * (1f / Mathf.Max(Vector3.Distance(transform.position, player.position), 1f));

        if (suspicionLevel >= suspicionThreshold)
        {
            animator.SetTrigger("isAlerted");
            StartChase();
        }
        else if (!IsPlayerVisible())
        {
            suspicionLevel -= Time.deltaTime;
            if (suspicionLevel <= 0)
            {
                suspicionIndicator.SetActive(false);
                agent.isStopped = false;
                currentState = EnemyState.Patrol;
            }
        }
    }

    // Handle the Chase state
    void HandleChase()
    {
        agent.isStopped = false;
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);

        if (!IsPlayerVisible())
        {
            disinterestTimer -= Time.deltaTime;
            if (disinterestTimer <= 0)
            {
                currentState = EnemyState.Return;
            }
        }
        else
        {
            if(disinterestTimer <= disinterestTimerMax ){
                disinterestTimer += Time.deltaTime;
            }
        }
    }

    // Handle the Return state
    void HandleReturn()
    {
        ResetAI();

        // Go to nearest patrol point instead of the first one
        Transform nearestPatrolPoint = GetNearestPatrolPoint();
        agent.SetDestination(nearestPatrolPoint.position);

        currentState = EnemyState.Patrol;
    }

    Transform GetNearestPatrolPoint()
    {
        Transform nearestPoint = patrolPoints[0];
        float shortestDistance = Vector3.Distance(transform.position, patrolPoints[0].position);

        foreach (var point in patrolPoints)
        {
            float distance = Vector3.Distance(transform.position, point.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestPoint = point;
            }
        }

        return nearestPoint;
    }

    // Start the chase
    void StartChase()
    {
        //isAlerted = true;
        suspicionIndicator.SetActive(false);
        disinterestTimer = disinterestTimerMax;
        killZone.enabled = true; // Enable kill zone
        currentState = EnemyState.Chase;
    }

    // Check if the player is in the field of view
    void CheckForPlayer()
    {
        if (IsPlayerVisible())
        {
            currentState = EnemyState.Suspicion;
        }
    }

    // Check if the player is visible
    bool IsPlayerVisible()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check front FoV
        if (Vector3.Angle(transform.forward, directionToPlayer) < frontFOVAngle / 2 && distanceToPlayer < fovRange)
        {
            if (!Physics.Linecast(transform.position, player.position, viewMask))
            {
                Debug.Log("I can see the player");
                return true;
            }
        }

        // Check back FoV
        if (Vector3.Angle(-transform.forward, directionToPlayer) < backFOVAngle / 2 && distanceToPlayer < backFOVRange)
        {
            if (!Physics.Linecast(transform.position, player.position, viewMask))
            {
                return true;
            }
        }

        return false;
    }

    // Reset all timers and states
    void ResetAI()
    {
        //isAlerted = false;
        suspicionLevel = 0;
        disinterestTimer = disinterestTimerMax;
        killZone.enabled = false; // Disable kill zone
        animator.ResetTrigger("isAlerted");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Player_Elara")){
            // Handle game over or player capture logic
            Debug.Log("Player caught!");
            PlayerManager.Instance.PlayerGameOver();
        }
        
    }
}
