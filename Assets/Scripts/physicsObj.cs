using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsObj : MonoBehaviour
{
    [SerializeField] int pushbackAmount;
    [SerializeField] bool push;
    Vector3 playerVelocity;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!push)
                gameManager.instance.playerScript.pushbackDir((transform.position - gameManager.instance.player.transform.position).normalized * pushbackAmount);
            else
                gameManager.instance.playerScript.pushbackDir((gameManager.instance.player.transform.position - transform.position).normalized * pushbackAmount);
        }
        
    }
}
