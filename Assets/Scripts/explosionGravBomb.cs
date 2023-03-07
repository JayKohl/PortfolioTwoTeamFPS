using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionGravBomb : MonoBehaviour
{
    [SerializeField] public int grenadeDamage;
    [SerializeField] int time;

    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip explosion;
    [Range(0, 1)] [SerializeField] float explosionVol;
    [SerializeField] int pushedBackAmount = 10;

    void Start()
    {
        StartCoroutine(timer(time));
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBoss"))
        {
            other.gameObject.GetComponent<enemyAI>().takeDamage(0);
            aud.PlayOneShot(explosion, explosionVol);
            StartCoroutine(other.gameObject.GetComponent<enemyOneAI>().pushedbackDir(transform.position));
        }
    }
    IEnumerator timer(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
