using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float chaseTime = 5f;

    private Transform player;
    private EnemyAIController aiController;
    private float chaseTimer = 0f;
    private bool isChasing = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        aiController = GetComponent<EnemyAIController>();
    }

    public void ChasePlayer()
    {
        isChasing = true;
        chaseTimer = 0f;
        GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(player.position);
    }

    public void HandleChase()
    {
        if (isChasing)
        {
            chaseTimer += Time.deltaTime;

            if (chaseTimer >= chaseTime)
            {
                isChasing = false;
                aiController.SetState(EnemyAIController.EnemyState.Returning);
            }
        }
    }
}
