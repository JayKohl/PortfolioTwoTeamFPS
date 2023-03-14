using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorTrapActivation : MonoBehaviour
{

	[SerializeField] floorTrap trapsType;

	[SerializeField]ParticleSystem trapVisual;
	public AudioClip soundEffect;
	public int activeTime;
	public int effectTime;
	public int damage;
	public bool trapActive;
	public Material trapMaterial;
	Color trapColor;


	//Effecrt Type
	// 1 = poison
	// 2 = electrecuted
	// 3 = burning
	// 4 = slow
	public int effectType;

	public GameObject triggerObject;
	public GameObject triggerObject1;
	public GameObject triggerObject2;

    private void Start()
    {
		trapColor = Color.red;
		trapColor.a = .5f;
		soundEffect = trapsType.soundEffect;
		activeTime = trapsType.activeTime;
		effectTime = trapsType.effectTime;
		damage = trapsType.damage;
		trapActive = trapsType.trapActive;
		effectType = trapsType.effectType;
		trapMaterial = trapsType.trapMaterial;
		trapMaterial = transform.GetComponent<Renderer>().material;

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
		yield return new WaitForSeconds(5);
		triggerObject.SetActive(true);
		trapMaterial.SetColor("_EmissionColor", Color.red * 1f);
		trapActive = true;	
		triggerObject.transform.position = Vector3.Lerp(triggerObject.transform.position, triggerObject2.transform.position, 1);     
        yield return new WaitForSeconds(5);
		triggerObject.SetActive(false);
		trapMaterial.SetColor("_EmissionColor", Color.black * 2);
		triggerObject.transform.position = Vector3.Lerp(triggerObject.transform.position, triggerObject1.transform.position, 1);
		trapActive = false;
		


	}

}
