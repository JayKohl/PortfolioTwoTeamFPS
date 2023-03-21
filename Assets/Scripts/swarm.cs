using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swarm : MonoBehaviour
{
    [SerializeField] float time = 5;
    [SerializeField] AudioSource aud;
    bool isTrue;

    private void Update()
    {
        if(gameManager.instance.isPaused)
        {
            isTrue = false;
            aud.Stop();
        }
        else if(!isTrue)
        {
            isTrue = true;
            aud.loop = true;
            aud.Play();
        }
    }

    private void Start()
    {
        aud.loop = true;
        aud.Play();
        StartCoroutine(timerDeath());
    }
    IEnumerator timerDeath()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
