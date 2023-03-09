using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newMissionOnTrigger : MonoBehaviour
{
    [SerializeField] string newGoal;

    private void OnTriggerEnter(Collider other)
    {
        gameManager.instance.infoText.text = "Patrol Not Found Proceed to Neighboring Crater";
        gameManager.instance.infoTextBackground.SetActive(true);
    }
}
