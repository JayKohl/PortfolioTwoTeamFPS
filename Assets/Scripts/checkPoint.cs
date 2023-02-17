using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //StartCoroutine(gameManager.instance.checkPointDisplay());
            gameManager.instance.playerSpawnPosition.transform.position = gameManager.instance.player.transform.position;
        }
    }
}
