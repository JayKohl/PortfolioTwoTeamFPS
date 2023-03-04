using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class secondCamera : MonoBehaviour
{
    [SerializeField] public Transform locDoorOne;
    [SerializeField] public Transform locDoorTwo;
    [SerializeField] public GameObject doorOne;
    [SerializeField] GameObject doorTwo;
    // Start is called before the first frame update
    Camera cam;

    public void openDoorOne()
    {       
        transform.position = locDoorOne.transform.position;        
        transform.Rotate(0,90,0);
    }
  
}
