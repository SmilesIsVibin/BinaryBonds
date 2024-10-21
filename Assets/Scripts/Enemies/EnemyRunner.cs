using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunner : MonoBehaviour
{
    public Transform player;
    public float baseChaseSpeed = 8f;
    private float currentChaseSpeed;

    void Update()
    {
        // Move towards the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > 0)
        {
            currentChaseSpeed = baseChaseSpeed;
            transform.position = Vector3.MoveTowards(transform.position, player.position, currentChaseSpeed * Time.deltaTime);
        }
    }

    public void OnTriggerEnter(Collider  other){
        if(other.CompareTag("Player")){
            GameManager.Instance.GameOverLevel();
        }
    }
}
