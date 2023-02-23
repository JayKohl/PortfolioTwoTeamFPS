using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endGame : MonoBehaviour
{
    bool playerIn;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playerIn)
            {
                playerIn = true;
                StartCoroutine(gameManager.instance.endLevel2());
            }
        }
    }
}
