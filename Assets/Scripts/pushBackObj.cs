using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushBackObj : MonoBehaviour
{
    //[SerializeField] public int missileDamage;
    [SerializeField] int pushBackDistance;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.pushbackDir((gameManager.instance.player.transform.position - transform.position).normalized * pushBackDistance);
        }
    }
}
