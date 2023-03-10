using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpointNPCLvl2 : MonoBehaviour
{
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
            StartCoroutine(waitMsg());
        }
    }
    IEnumerator waitMsg()
    {
        yield return new WaitForSeconds(3);
        gameManager.instance.displayNpcText("You have triggered the alarm! Take out the console on the second floor!");
        StartCoroutine(gameManager.instance.deleteTextNpc(3));
    }
}