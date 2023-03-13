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
        aud.PlayOneShot(menuAud, menuAudVol);
        StartCoroutine(fadeMusic());
    }

    // Update is called once per frame
    IEnumerator fadeMusic()
    {
        float time = 0;
        float start = menuAudVol;
        while (time < 48)
        {
            time += Time.deltaTime;
            aud.volume = Mathf.Lerp(start, 0, time / 48);
            yield return null;
        }
    }
}
