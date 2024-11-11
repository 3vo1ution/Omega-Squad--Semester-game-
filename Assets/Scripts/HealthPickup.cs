using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public GameObject sicklyFilter;
    public float IncreaseAmount;
    public AudioManager audioManager;
    private void OnTriggerEnter(Collider other)// this function is used when a trigger collider comes in contact with another collider, the other refers to the other collider
    {
        if (other.CompareTag("Player"))// bool to check if the object that has collided with the health pickup has the tag player
        {
            other.gameObject.GetComponent<HealthManager>().HealthIncrease(IncreaseAmount);// to access the HeallthIncrease method in the health manager script
            sicklyFilter.SetActive(false);
            Debug.Log("sickly off");
        }
        Destroy(gameObject);// get rid of health after player passes through it

        if (other.CompareTag("Player"))
        {
            audioManager.PlayItemPickupSound();
            Destroy(gameObject);
        }


    }




}
