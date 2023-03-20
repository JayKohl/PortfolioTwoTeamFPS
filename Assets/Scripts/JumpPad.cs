using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] Vector3 velocity;
    [Range(2, 100)] [SerializeField] int jumpPadHeight;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] jumpPadAud;
    [Range(0, 1)] [SerializeField] float audJumpVol;    
    bool playerIn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerIn)
        {
            playerIn = true;
            gameManager.instance.playerScript.jumpsCurrent++; 
            gameManager.instance.playerScript.playerVelocity.y = jumpPadHeight;
            gameManager.instance.playerScript.controller.Move((velocity) * Time.deltaTime);
            //playerVelocity.y = jumpPadHeight;
            //gameManager.instance.playerScript.pushbackDir(new Vector3 (0, jumpPadHeight, 0));
            //gameManager.instance.playerScript.controller.Move((velocity + new Vector3(0, jumpPadHeight, 0)) * Time.deltaTime);
            aud.PlayOneShot(jumpPadAud[Random.Range(0, jumpPadAud.Length)], audJumpVol);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerIn)
        {
            playerIn = false;
            //gameManager.instance.playerScript.playerVelocity.y = jumpPadHeight;
        }
    }
}
