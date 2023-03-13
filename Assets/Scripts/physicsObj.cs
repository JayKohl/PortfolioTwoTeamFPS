using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsObj : MonoBehaviour
{
    [SerializeField] int pushbackAmount;
    [SerializeField] bool push;
    [SerializeField] AudioSource aud;
    Vector3 playerVelocity;
    bool playerIn;

    [Header("----- Sound -----")]
    [SerializeField] AudioClip[] audWindTunnel;
    [Range(0, 1)] [SerializeField] float audWindTunnelVol;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerIn)
        {
            if (!push)
                gameManager.instance.playerScript.pushbackDir((transform.position - gameManager.instance.player.transform.position).normalized * pushbackAmount);
            else
                gameManager.instance.playerScript.pushbackDir((gameManager.instance.player.transform.position - transform.position).normalized * pushbackAmount);
        }
        //playerIn = true;
        aud.PlayOneShot(audWindTunnel[Random.Range(0, audWindTunnel.Length)], gameManager.instance.soundVol);
    }
}
