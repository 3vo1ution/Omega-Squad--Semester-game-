using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public float damageAmount = 10f; // Amount of damage dealt to the player
    public HealthManager healthManager;

    void Start()
    {
        Debug.Log("EnemyCollision script is active.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthManager healthmanager = collision.gameObject.GetComponent<HealthManager>();

            if (healthManager != null)
            {
                healthmanager.HealthDecrease(damageAmount);
            }

        }
    }
}