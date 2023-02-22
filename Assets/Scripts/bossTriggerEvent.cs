using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossTriggerEvent : MonoBehaviour
{
    [SerializeField] GameObject laserField;
    bool playerIn;

    private void Start()
    {
        laserField.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!playerIn)
        {
            if (other.CompareTag("Player"))
            {
                playerIn = true;
                laserField.SetActive(true);
            }
        }
    }
}
