using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField] Transform radar;
    float radarSpeed;
    // Start is called before the first frame update
    void Awake()
    {
       
        radarSpeed = 180f;
    }

    // Update is called once per frame
    void Update()
    {
        radar.eulerAngles -= new Vector3(0, 0, radarSpeed * Time.deltaTime);
    }
}
