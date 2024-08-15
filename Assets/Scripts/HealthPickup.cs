using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    public float IncreaseAmount;
    private void OnTriggerEnter(Collider other)// this function is used when a trigger collider comes in contact with another collider, the other refers to the other collider
    {
      if (other.CompareTag("Player"))// bool to check if the object that has collided with the health pickup has the tag player
        {
            other.gameObject.GetComponent<HealthManager>().HealthIncrease(IncreaseAmount);// to access the HeallthIncrease method in the health manager script
        }
        Destroy(gameObject);// get rid of health after player passes through it

       

    }




}
