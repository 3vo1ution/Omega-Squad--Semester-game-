using System.Collections;
using UnityEngine;

public class SugarRushPickup : MonoBehaviour
{
    public FirstPersonControls FirstPersonControls;

    public GameObject sugarRushFilter;
 
    private void OnTriggerEnter(Collider other)// this function is used when a trigger collider comes in contact with another collider, the other refers to the other collider
    {
        if (other.CompareTag("Player"))// bool to check if the object that has collided with the health pickup has the tag player
        {
            //FirstPersonControls.ActivateSpeedBoost();
            sugarRushFilter.SetActive(true);
            Debug.Log("SugarRush");
            StartCoroutine(StopSugarRush());
        }
        Destroy(gameObject);// get rid of health after player passes through it

    }

    public IEnumerator StopSugarRush()
    {
        yield return new WaitForSeconds(5f);
        sugarRushFilter.SetActive(false);
    }



}
