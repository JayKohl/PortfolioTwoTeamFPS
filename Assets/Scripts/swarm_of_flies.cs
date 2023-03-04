using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swarm_of_flies : MonoBehaviour
{
    [SerializeField] float timer;
    [SerializeField] GameObject swarmOfFlies;
    
    void Start()
    {
        StartCoroutine(explode());        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(swarmOfFlies, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    IEnumerator explode()
    {
        yield return new WaitForSeconds(timer);
        Instantiate(swarmOfFlies, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
