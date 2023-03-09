using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newMissionOnTrigger : MonoBehaviour
{
    [SerializeField] string newGoal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.infoText.text = "Patrol Not Found \n Proceed to Neighboring Crater";
            gameManager.instance.infoTextBackground.SetActive(true);
        }
    }
}
