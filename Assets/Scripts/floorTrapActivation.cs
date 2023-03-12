using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorTrapActivation : MonoBehaviour
{

	[SerializeField] floorTrap trapsType;

	public ParticleSystem trapVisual;
	public AudioClip soundEffect;
	public int activeTime;
	public int effectTime;
	public int damage;
	public bool trapActive = false;

	//Effecrt Type
	// 1 = poison
	// 2 = electrecuted
	// 3 = burning
	// 4 = slow
	public int effectType;
	float time;
    private void Start()
    {
		time = 0;
		trapVisual = trapsType.trapVisual;
		soundEffect = trapsType.soundEffect;
		activeTime = trapsType.activeTime;
		effectTime = trapsType.effectTime;
		damage = trapsType.damage;
		trapActive = trapsType.trapActive;
		effectType = trapsType.effectType;
		
	}
    void Update()
	{
		if(time <= 0)
        {
			trapActive = false;
			time -= time * Time.deltaTime;
        }
		else
        {
			trapActive = true;
        }

	}
    private void OnTriggerEnter(Collider other)
    {

		if (other.CompareTag("Player") && trapActive)
		{

			switch (effectType)
			{
				case (1):
					gameManager.instance.playerScript.Poisoned(effectTime, damage, soundEffect);
					break;
				case (2):
					gameManager.instance.playerScript.Electrecuted(effectTime, damage, soundEffect);
					break;
				case (3):
					gameManager.instance.playerScript.Burning(effectTime, damage, soundEffect);
					break;
				case (4):
					StartCoroutine(gameManager.instance.playerScript.Slowed(effectTime, damage));
					break;
				default:
					break;
			}

		}
		
	}

	IEnumerator TrapCycle()
	{
		//trapActive = true;
		//transform.GetComponent<Renderer>().material.color = Color.red;
		//Debug.Log("here");
		yield return new WaitForSeconds(5);
		//Debug.Log("I'm here");
		//transform.GetComponent<Renderer>().material.color = Color.blue;
		//trapActive = false;
		//yield return new WaitForSeconds(5);
		//Debug.Log("I'm Out");
	}

}
