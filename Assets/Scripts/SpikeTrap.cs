using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] GameObject spikes;

    [Header("----- Sound -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] spikeAud;
    [Range(0, 1)] [SerializeField] float audspikeVol;

    //[Header("----- Timer -----")]
    //[Range(0, 5)] [SerializeField] float spikeTimer;

    //[Header("----- Damage -----")]
    //[Range(0, 100)] [SerializeField] int spikeDamage;

    bool playerIn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerIn)
        {
            playerIn = true;
            spikes.transform.position = spikes.transform.position + new Vector3(0, 1f, 0);
            StartCoroutine(spikesUp());
        }
    }

    IEnumerator spikesUp()
    {
        yield return new WaitForSeconds(.5f);
        aud.PlayOneShot(spikeAud[Random.Range(0, spikeAud.Length)], audspikeVol);
        playerIn = false;
        spikes.transform.position = spikes.transform.position + new Vector3(0, -1f, 0);
        gameManager.instance.playerScript.takeDamage(100);
    }
}
