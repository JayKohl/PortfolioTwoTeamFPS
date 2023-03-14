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
            gameManager.instance.playerCamera.GetComponentInChildren<Objectivepoint>().SetWayPoint(gameManager.instance.playerCamera.GetComponentInChildren<Objectivepoint>().quest3);
            gameManager.instance.infoText.text = "Patrol Not Found \n Proceed to Neighboring Crater";
            gameManager.instance.infoTextBackground.SetActive(true);
        }
    }
}
