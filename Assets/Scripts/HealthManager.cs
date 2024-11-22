using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [SerializeField] float MaxHealth;
    private float currentHealth;
    public float DecreaseInterval = 15f; // Health decreases every 15 seconds
    public float HealthDecreaseAmount = 10f; // Health decreases by 10 every 15 seconds
    public float timer;

    public HealthBar healthBar;
    public GameObject sicklyFilter;

    private void Start()
    {
        currentHealth = MaxHealth;

        healthBar.SetMaxHealth(currentHealth); // Set the max health of the player when the game starts

        timer = DecreaseInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            HealthDecrease(HealthDecreaseAmount);
            timer = DecreaseInterval;
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        if (currentHealth <= 40)
        {
            sicklyFilter.SetActive(true);
        }
        else
        {
            sicklyFilter.SetActive(false);
        }
    }

    public void HealthDecrease(float health)
    {
        currentHealth -= health; // Subtract from the current health

        if (currentHealth < 0f) // Prevent health from going lower than zero
        {
            currentHealth = 0f;
        }

        healthBar.SetSlider(currentHealth); // Update slider

        Debug.Log("Health: " + currentHealth);
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount; // Subtract damage from current health

        if (currentHealth < 0f) // Ensure health doesn't go below zero
        {
            currentHealth = 0f;
        }

        healthBar.SetSlider(currentHealth); // Update the health bar slider

        Debug.Log($"Health decreased. Current health: {currentHealth}");
    }


    public void HealthIncrease(float health)
    {
        currentHealth += health;

        if (currentHealth > MaxHealth)
        {
            currentHealth = MaxHealth; // Prevent health from exceeding max health
        }

        healthBar.SetSlider(currentHealth);

        Debug.Log("Health: " + currentHealth);
    }

    private void Die()
    {
        SceneManager.LoadScene("StartingScene");
    }
}
