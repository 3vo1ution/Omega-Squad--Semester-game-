
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    public Transform player; // Assign the player in the Inspector
    public float attackRange = 2f; // Range within which the enemy attacks
    public float chaseSpeed = 3.5f;

    private NavMeshAgent agent;
    private bool playerInZone = false;

    public int damage;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = chaseSpeed; // Set the chase speed
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (playerInZone)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Chase the player
            if (distanceToPlayer > attackRange)
            {
                agent.SetDestination(player.position);
            }
            // Stop moving and attack if in range
            else
            {
                agent.ResetPath();
                AttackPlayer();
            }
        }
    }

    void AttackPlayer()
    {
        // Play attack animation or logic here
        Debug.Log("Enemy is attacking the player!");
    }

    public void SetPlayerInZone(bool inZone)
    {
        playerInZone = inZone;
    }


}
    

