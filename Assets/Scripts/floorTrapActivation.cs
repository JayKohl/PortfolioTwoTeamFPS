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
	public bool trapActive;
	public GameObject damageArea;
	//Effecrt Type
	// 1 = poison
	// 2 = electrecuted
	// 3 = burning
	// 4 = slow
	public int effectType;
	float time;
    private void Start()
    {
		damageArea = trapsType.damageArea;
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
		if(trapActive == false)
        {
			StartCoroutine(TrapCycle());
        }
	}

	IEnumerator TrapCycle()
	{
		Instantiate(damageArea);
		yield return new WaitForSeconds(5);
		trapActive = true;
        transform.GetComponent<Renderer>().material.color = Color.red;
        Debug.Log("here");
        yield return new WaitForSeconds(5);
		Destroy(damageArea);
        Debug.Log("I'm here");
        transform.GetComponent<Renderer>().material.color = Color.blue;
        trapActive = false;
        
    }

}
