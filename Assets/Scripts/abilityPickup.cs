using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abilityPickup : MonoBehaviour
{
    bool playerIn;
    [SerializeField] abilities stats;
    [SerializeField] AudioClip pickupSound;
    [SerializeField][Range (0,1)] float pickupSoundVol = 0.2f;
    AudioSource aud;

    private void Start()
    {
        aud = gameManager.instance.aud;
        StartCoroutine(rotate());
    }
    private IEnumerator rotate()
    {
        while (true)
        {
            if (!gameManager.instance.isPaused)
            {
                transform.Rotate(0f, 0.5f, 0f, Space.Self);
            }
            yield return new WaitForSeconds(1f / 60);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerIn)
        {
            aud.PlayOneShot(pickupSound, gameManager.instance.soundVol);
            playerIn = true;
            gameManager.instance.abilityHub.GetComponent<activateAbility>().abilityPickup(stats);
            gameManager.instance.pause();
            Destroy(gameObject);
        }
    }
}