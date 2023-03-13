using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicMenu : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] public AudioClip menuAud;
    [Range(0, 1)] [SerializeField] public float menuVol;

    void Start()
    {
        aud.PlayOneShot(menuAud, gameManager.instance.musicVol);
    }
}
