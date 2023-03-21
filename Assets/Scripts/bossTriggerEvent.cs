using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossTriggerEvent : MonoBehaviour, IDamage
{
    [SerializeField] GameObject laserField;
    bool playerIn;

    [SerializeField] BoxCollider noShootBoss;
    bool isPlayerIn;

    private void Start()
    {
        laserField.SetActive(false);
    }
    public virtual void takeDamage(int dmg)
    {
        if (!isPlayerIn)
        {
            // this is to catch ray casts so that the player cannot shoot the boss
            // from outside the boss' arena. 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!playerIn)
        {
            if (other.CompareTag("Player"))
            {
                playerIn = true;
                laserField.SetActive(true);
                gameManager.instance.flightDeck = true;
                gameManager.instance.infoText.text = "<s>Get to the flight deck</s>"+"\nKill the radiated bug";
                gameManager.instance.infoTextBackground.SetActive(true);
                isPlayerIn = true;
                noShootBoss.enabled = false;
            }
        }
    }
}
