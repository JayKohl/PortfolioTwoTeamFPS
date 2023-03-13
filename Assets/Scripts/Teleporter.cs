using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    
    public GameObject TeleportTo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameManager.instance.player.transform.position = TeleportTo.transform.position;
            
        }
    }
}
