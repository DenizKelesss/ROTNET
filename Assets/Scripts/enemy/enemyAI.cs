using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float chaseRange = 10f; // Distance at which the enemy starts chasing
    public float damageRange = 2f; // Distance at which the enemy can deal damage
    public float moveSpeed = 3f; // How fast the enemy moves
    public float damage = 10f; // Damage dealt to the player when in range

    private NavMeshAgent agent; // NavMeshAgent for pathfinding
    private float distanceToPlayer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;  // Assumes the player is tagged as "Player"
        }
    }

    private void Update()
    {
        // Calculate the distance between the enemy and the player
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If the player is within the chase range, the enemy should move toward the player
        if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }

        // If the player is within the damage range, the enemy should deal damage
        if (distanceToPlayer <= damageRange)
        {
            DealDamage();
        }
    }

    // Method to make the enemy chase the player
    void ChasePlayer()
    {
        agent.SetDestination(player.position);  // Move the enemy towards the player
        agent.speed = moveSpeed; // Set the movement speed of the enemy
    }

    // Method to deal damage to the player when in range
    void DealDamage()
    {
        // Assume the player has a "PlayerHealth" script to take damage
        playerHealth playerHealth = player.GetComponent<playerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
