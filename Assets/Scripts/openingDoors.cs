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
        doorOne.SetActive(false);
        yield return new WaitForSeconds(1);
    }
}
