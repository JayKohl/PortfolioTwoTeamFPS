using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] spikeAud;
    [Range(0, 1)] [SerializeField] float audspikeVol;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.takeDamage(10);
            aud.PlayOneShot(spikeAud[Random.Range(0, spikeAud.Length)], audspikeVol);
        }
    }
}
