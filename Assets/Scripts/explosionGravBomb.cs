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
            if (other.GetComponent<IDamage>() != null)
            {
                target = other.gameObject;
                target.GetComponent<enemyAI>().takeDamage(0);
                aud.PlayOneShot(explosion);
                StartCoroutine(other.gameObject.GetComponent<enemyAI>().pushedbackDir(transform.position));
                aud.PlayOneShot(gravityEffect, gameManager.instance.soundVol);
            }
        }
    }
    IEnumerator timer(float time)
    {
        yield return new WaitForSeconds(time);
        if (target != null)
        {
            target.GetComponent<enemyAI>().gravBombEnd();
        }
        Destroy(gameObject);
    }
}
