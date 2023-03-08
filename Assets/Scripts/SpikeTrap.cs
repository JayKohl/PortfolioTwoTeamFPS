using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] GameObject spikes;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] spikeAud;
    [Range(0, 1)] [SerializeField] float audspikeVol;
    bool playerIn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerIn)
        {
            playerIn = true;
            spikes.transform.position = spikes.transform.position + new Vector3(0, .5f, 0);
            StartCoroutine(spikesUp());
        }
    }
    IEnumerator spikesUp()
    {
        aud.PlayOneShot(spikeAud[Random.Range(0, spikeAud.Length)], audspikeVol);
        yield return new WaitForSeconds(.5f);
        playerIn = false;
        spikes.transform.position = spikes.transform.position + new Vector3(0, -.5f, 0);
        gameManager.instance.playerScript.takeDamage(50);
    }
}
