using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public float MaxHealth;



    public void SetSlider(float health)
    {
        healthSlider.value = health;
        Debug.Log($"Health bar updated: {health}");

    }

    public void SetMaxHealth(float health)
    {
        healthSlider.maxValue = health;
        SetSlider(health);
    }

   

}
