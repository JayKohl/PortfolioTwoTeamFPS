using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicPart2 : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] public AudioClip menuAud;
    [SerializeField] public float menuAudVol;

    void Start()
    {
        aud.PlayOneShot(menuAud, menuAudVol);
    }
}
