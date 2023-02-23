using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicCrawl : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] public AudioClip menuAud;
    [Range(0, 1)] [SerializeField] public float menuVol;

    void Start()
    {
        aud.PlayOneShot(menuAud, menuVol);
    }
}
