using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorTrigger : MonoBehaviour
{
	floorTrapActivation trapParent;
	bool playerIn;

    private void Start()
    {
        trapParent = GetComponentInParent<floorTrapActivation>();
    }
    private void OnTriggerEnter(Collider other)
	{
		if(!playerIn)
        {
			if (other.CompareTag("Player") && trapParent.trapActive)
			{
				playerIn = true;
				switch (trapParent.effectType)
				{
					case (1):
						if (gameManager.instance.playerScript.poisoned == false)
						{
							gameManager.instance.playerScript.poisoned = true;
							StartCoroutine(gameManager.instance.playerScript.Poisoned(trapParent.effectTime, trapParent.damage, trapParent.soundEffect));
							//gameManager.instance.playerScript.poisoned = false;
						}

						break;
					case (2):
						if (gameManager.instance.playerScript.electrecuted == false)
						{
							gameManager.instance.playerScript.electrecuted = true;
							StartCoroutine(gameManager.instance.playerScript.Electrecuted(trapParent.effectTime, trapParent.damage, trapParent.soundEffect));
							//gameManager.instance.playerScript.electrecuted = false;
						}
						break;
					case (3):

						if (gameManager.instance.playerScript.burning == false)
						{
							gameManager.instance.playerScript.burning = true;
							StartCoroutine(gameManager.instance.playerScript.Burning(trapParent.effectTime, trapParent.damage, trapParent.soundEffect));
							//gameManager.instance.playerScript.burning = false;
						}
	
						break;
					case (4):
						if (gameManager.instance.playerScript.slowed == false)
						{
							gameManager.instance.playerScript.slowed = true;
							StartCoroutine(gameManager.instance.playerScript.Slowed(trapParent.effectTime, trapParent.damage));
							//gameManager.instance.playerScript.slowed = false;
						}
						
						break;
					default:
						break;
				}
				
			}
		}

	}
	private void OnTriggerExit(Collider other)
    {
		playerIn = false;
    }
}
