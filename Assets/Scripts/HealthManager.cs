using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [SerializeField] public float MaxHealth;
    private float currentHealth;
    private float timeElapsed = 0f;//time since last decrease
    public float DecreaseInterval = 15f;// health decreases every 15 seconds
    public float HealthDecreaseAmount = 10f;//health decreases by 10 every 15 seconds
    public float timer;

    public HealthBar healthBar;


    private void Start()
    {
        currentHealth = MaxHealth;

        healthBar.SetMaxHealth(currentHealth);//set the max health of the player when the game starts

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

        if (currentHealth <=0)
        {
            Die();
        }
    }



    public void HealthDecrease(float health)
    {
        currentHealth -= health;// we are subtracting from the current health which is set in the starting method

        if (currentHealth < 0f) //prevent health from going lower than zero
        {
            currentHealth = 0f;

        }

        healthBar.SetSlider(currentHealth);// update slider

        Debug.Log("Health:" + currentHealth);


    }

    public void HealthIncrease(float health)
    {
        if (currentHealth > MaxHealth)
        {
            currentHealth = MaxHealth;// To make sure health does not go over the max health
        }

        currentHealth += health;// we are adding to the current health which is set in the starting method

        healthBar.SetSlider(currentHealth);

        Debug.Log("Health:" + currentHealth);

    }

    private void Die()
    {
        SceneManager.LoadScene("StartingScene");
    }


}
