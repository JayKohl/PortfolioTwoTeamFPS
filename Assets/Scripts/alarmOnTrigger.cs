using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alarmOnTrigger : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip alarm;

    [SerializeField] GameObject alarmLights;

    bool lightOn;
    bool playerIn;
    int counter;

    private void Start()
    {
        alarmLights.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!playerIn)
        {
            if (other.CompareTag("Player"))
            {
                counter = 0;
                playerIn = true;
                aud.PlayOneShot(alarm);
            }
        }
    }
    public void alarmOff()
    {
        aud.Stop();
        alarmLights.SetActive(false);
    }
    private void Update()
    {
        if (!playerIn)
            return;
        counter++;        
        if(counter >= 60)
        {
            counter = 0;
            lightOn = !lightOn;
            if(lightOn)
            {
                alarmLights.SetActive(true);
            }
            else
            {
                alarmLights.SetActive(false);
            }
        }
    }
}
