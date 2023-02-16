using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("----- Missile Info -----")]
    public int missileDamage;
    [SerializeField] int maxTravelTime;
    //[SerializeField] GameObject missileModel;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, maxTravelTime);
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.takeDamage(missileDamage);
        }
        Destroy(gameObject);
    }
}
