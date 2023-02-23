using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundSound : MonoBehaviour
{
    bool playerIn;
    [SerializeField] bool isDirt;
    private void OnTriggerEnter(Collider other)
    {
        if(!playerIn)
        {
            playerIn = true;
            if(isDirt)
            {
                gameManager.instance.playerScript.dirt = true;
            }
            else
            {
                gameManager.instance.playerScript.dirt = false;
            }
        }
    }
}
