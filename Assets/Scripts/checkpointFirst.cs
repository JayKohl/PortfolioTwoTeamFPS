using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class checkpointFirst : MonoBehaviour
{
    [SerializeField] notifications texture;
    bool playerIn;
    private void Start()
    {
        playerIn = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerIn)
        {
            playerIn = true;
            StartCoroutine(gameManager.instance.notificationDisplay(texture));
            gameManager.instance.playerSpawnPosition.transform.position = transform.position;
            StartCoroutine(checkPointIntro());
        }
    }
    IEnumerator checkPointIntro()
    {
        gameManager.instance.displayNpcText("Checkpoint found! You will return here if you hit Respawn after dying. \n                           [All current progress will be saved.]");
        StartCoroutine(gameManager.instance.deleteTextNpc(3));
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
