using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorTrapActivation : MonoBehaviour
{

	public floorTrap trapsType;

	
	public GameObject triggerObject;
	public GameObject triggerObject1;
	public GameObject triggerObject2;

    private void Start()
    {
		Instantiate(trapsType.trapVisual );
	}
    void Update()
	{
		if(trapsType.trapActive == false)
        {
			StartCoroutine(TrapCycle());
        }
	}

	IEnumerator TrapCycle()
	{
		yield return new WaitForSeconds(5);
		
		trapsType.trapActive = true;
        transform.GetComponent<Renderer>().material.color = Color.red;
		triggerObject.transform.position = Vector3.Lerp(triggerObject.transform.position, triggerObject2.transform.position, 1);
       
        yield return new WaitForSeconds(5);
		
        transform.GetComponent<Renderer>().material.color = Color.blue;
		triggerObject.transform.position = Vector3.Lerp(triggerObject.transform.position, triggerObject1.transform.position, 1);
		trapsType.trapActive = false;
		

	}

}
