using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public float MaxHealth;



    public void SetSlider(float health)
    {
        healthSlider.value = health;
    }

    public void SetMaxHealth(float health)
    {
        healthSlider.maxValue = health;
        SetSlider(health);
    }


}
