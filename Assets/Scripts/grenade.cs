using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour
{
    [SerializeField] int timer;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject explosion;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(timer);
        if(explosionEffect)
        {
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            Instantiate(explosionEffect, transform.position, explosion.transform.rotation);
        }
        Destroy(gameObject);
    }
}
