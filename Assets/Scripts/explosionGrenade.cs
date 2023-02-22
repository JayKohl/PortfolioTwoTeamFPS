using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionGrenade : MonoBehaviour
{
    [SerializeField] public int grenadeDamage;
    [SerializeField] int pushBackDistance;
    [SerializeField] int time;
    bool playerIn;
    void Start()
    {
        StartCoroutine(timer(time));
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBoss"))
        {
            other.GetComponent<enemyTurretAI>().takeDamage(grenadeDamage);
        }
    }
    IEnumerator timer(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
