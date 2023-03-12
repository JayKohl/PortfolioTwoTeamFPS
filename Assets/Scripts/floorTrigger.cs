using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorTrigger : MonoBehaviour
{
	floorTrapActivation trapParent;

    private void Start()
    {
        trapParent = GetComponentInParent<floorTrapActivation>();
    }
    private void OnTriggerEnter(Collider other)
	{


		if (other.CompareTag("Player") && trapParent.trapActive)
		{

			switch (trapParent.effectType)
			{
				case (1):
					if (gameManager.instance.playerScript.poisoned == false)
					{
						gameManager.instance.playerScript.poisoned = true;
						StartCoroutine(gameManager.instance.playerScript.Poisoned(trapParent.effectTime, trapParent.damage, trapParent.soundEffect));
					}

					break;
				case (2):
					if (gameManager.instance.playerScript.electrecuted == false)
					{
						gameManager.instance.playerScript.electrecuted = true;
						StartCoroutine(gameManager.instance.playerScript.Electrecuted(trapParent.effectTime, trapParent.damage, trapParent.soundEffect));
					}
					break;
				case (3):
					gameManager.instance.playerScript.Burning(trapParent.effectTime, trapParent.damage, trapParent.soundEffect);
					break;
				case (4):
					StartCoroutine(gameManager.instance.playerScript.Slowed(trapParent.effectTime, trapParent.damage));
					break;
				default:
					break;
			}


		}

	}
}
