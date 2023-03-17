using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour
{
    [SerializeField] float timer;
    float time;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject explosion;
    IEnumerator Start()
    {
        time = timer;
        while (time > 0)
        {
            transform.Rotate(2f, 2f, 0f, Space.Self);
            yield return new WaitForSeconds(.01f);
            time-=.01f;            
        }
        if (explosionEffect)
        {
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            Instantiate(explosionEffect, transform.position, explosion.transform.rotation);
        }
        Destroy(gameObject);
    }
}
