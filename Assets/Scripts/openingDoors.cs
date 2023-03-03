using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openingDoors : MonoBehaviour
{
    [SerializeField] public Transform doorCamOne;
    


    private void Start()
    {
        
    }
    public IEnumerator OpenDoorOne(GameObject doorOne)
    {
        doorOne.transform.position = Vector3.Lerp(doorOne.transform.position, doorOne.transform.position + new Vector3(0, -5, 0), 2 * Time.deltaTime);
        //doorOne.SetActive(false);
        yield return new WaitForSeconds(2);
    }
}
