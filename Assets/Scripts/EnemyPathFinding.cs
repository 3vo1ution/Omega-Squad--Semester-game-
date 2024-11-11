using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : MonoBehaviour
{
    public Transform player;          // Reference to the player
    private NavMeshAgent agent;       // NavMeshAgent to move the enemy
    private bool isChasing = false;   // Whether the enemy is chasing the player

    public float chaseRange = 10f;    // The range at which the enemy will start chasing
    public float damageAmount = 10f;  // The damage amount when colliding with the player

    public float rotAreaRadius = 20f; // The radius of the rot area
    public float decreaseAmount = 10f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is missing on this enemy object.");
        }
    }

    void Update()
    {
        if (agent.isOnNavMesh && agent.enabled)
        {
            if (IsPlayerInRotArea() && Vector3.Distance(transform.position, player.position) <= chaseRange)
            {
                if (!isChasing)
                {
                    Debug.Log("Enemy started chasing the player.");
                    // Potentially trigger animations or sound effects
                }
                isChasing = true;
                agent.SetDestination(player.position);
            }
            else
            {
                if (isChasing)
                {
                    Debug.Log("Enemy stopped chasing the player.");
                    // Potentially stop animations or sound effects
                }
                isChasing = false;
                agent.ResetPath();
            }
        }
        else
        {
            Debug.LogWarning("Enemy's NavMeshAgent is either not on a NavMesh or disabled.");
        }
    }


    bool IsPlayerInRotArea()
    {
        // Check if the player is inside the rot area radius
        float distanceToRotArea = Vector3.Distance(transform.position, player.position);
        return distanceToRotArea <= rotAreaRadius;  // Player is within the rot area
    }

    /* void OnCollisionEnter(Collision collision)
     {
         if (collision.gameObject.CompareTag("Player") && isChasing == true)
         {
             Debug.Log("enemyhitting");
             // Access the player's HealthManager and apply damage
             HealthManager healthManager = collision.gameObject.GetComponent<HealthManager>();
             if (healthManager != null)
             {
                 healthManager.HealthDecrease(damageAmount);  // Deal damage
             }
         }
     } 
    */

    private void OnTriggerEnter(Collider other)// this function is used when a trigger collider comes in contact with another collider, the other refers to the other collider
    {
        if (other.CompareTag("Player"))// bool to check if the object that has collided with the health pickup has the tag player
        {
            other.gameObject.GetComponent<HealthManager>().HealthDecrease(decreaseAmount);// to access the HeallthIncrease method in the health manager script
           
        }
        


    }

}
