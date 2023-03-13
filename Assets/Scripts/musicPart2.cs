using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicPart2 : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] public AudioClip menuAud;

    void Start()
    {
        aud.PlayOneShot(menuAud, gameManager.instance.musicVol);
    }
}
