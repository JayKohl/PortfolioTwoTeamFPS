using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class checkPoint : MonoBehaviour
{
    bool playerIn;
    private void Start()
    {
        playerIn = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !playerIn)
        {
            playerIn = true;
            gameManager.instance.displayNpcText("Checkpoint");
            gameManager.instance.playerSpawnPosition.transform.position = transform.position;
        }
    }
}
