using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorTrapDamage : MonoBehaviour
{
	floorTrapActivation trapScript;

    private void Start()
    {
        trapScript = GetComponentInParent<floorTrapActivation>();
    }
    private void OnTriggerEnter(Collider other)
	{

		if (other.CompareTag("Player") && trapScript.trapActive)
		{

			switch (trapScript.effectType)
			{
				case (1):
					gameManager.instance.playerScript.Poisoned(trapScript.effectTime, trapScript.damage, trapScript.soundEffect);
					break;
				case (2):
					gameManager.instance.playerScript.Electrecuted(trapScript.effectTime, trapScript.damage, trapScript.soundEffect);
					break;
				case (3):
					gameManager.instance.playerScript.Burning(trapScript.effectTime, trapScript.damage, trapScript.soundEffect);
					break;
				case (4):
					StartCoroutine(gameManager.instance.playerScript.Slowed(trapScript.effectTime, trapScript.damage));
					break;
				default:
					break;
			}


		}

	}
}
