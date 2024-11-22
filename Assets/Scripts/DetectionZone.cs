using UnityEngine;


public class DetectionZone : MonoBehaviour
{
    public EnemyAI enemyAI; // Assign the enemy AI script in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAI.SetPlayerInZone(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAI.SetPlayerInZone(false);
        }
    }
}
