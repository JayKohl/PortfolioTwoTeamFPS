using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicCredits : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] public AudioClip menuAud;
    [SerializeField] public float menuAudVol;

    void Start()
    {
        aud.loop = true;
        aud.Play();        
    }
}
