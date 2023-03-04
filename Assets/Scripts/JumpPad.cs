using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    Vector3 playerVelocity;
    [Range(2, 10)] [SerializeField] int jumpPadHeight;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] jumpPadAud;
    [Range(0, 1)] [SerializeField] float audJumpVol;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //playerVelocity.y = jumpPadHeight;
            gameManager.instance.playerScript.pushbackDir(new Vector3 (0, jumpPadHeight, 0));
            aud.PlayOneShot(jumpPadAud[Random.Range(0, jumpPadAud.Length)], audJumpVol);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.pushbackDir(new Vector3(0, 0, 0));
        }
    }
}
