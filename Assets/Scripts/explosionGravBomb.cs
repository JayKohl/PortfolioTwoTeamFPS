using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionGravBomb : MonoBehaviour
{
    [SerializeField] public int grenadeDamage;
    [SerializeField] int time;

    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip gravityEffect;

    GameObject target;

    void Start()
    {
        StartCoroutine(timer(time));
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBoss"))
        {
            target = other.gameObject;
            target.GetComponent<enemyAI>().takeDamage(0);
            aud.PlayOneShot(explosion, 0.2f);
            StartCoroutine(other.gameObject.GetComponent<enemyOneAI>().pushedbackDir(transform.position));
            aud.PlayOneShot(gravityEffect, gameManager.instance.soundVol);
        }
    }
    IEnumerator timer(float time)
    {
        yield return new WaitForSeconds(time);
        target.GetComponent<enemyAI>().gravBombEnd();
        Destroy(gameObject);
    }
}
