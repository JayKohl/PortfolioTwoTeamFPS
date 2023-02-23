using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundSound : MonoBehaviour
{
    [SerializeField] bool isDirt;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
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
