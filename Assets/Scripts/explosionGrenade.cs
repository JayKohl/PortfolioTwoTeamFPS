using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionGrenade : MonoBehaviour
{
    [SerializeField] public int grenadeDamage;
    [SerializeField] int time;

    void Start()
    {
        StartCoroutine(timer(time));
    }
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Enemy") || other.CompareTag("EnemyBoss"))
    //    {
    //        other.gameObject.GetComponent<enemyAI>().takeDamage(grenadeDamage);
    //    }
    //} This part commented out because it hits EVERY enemy in the scene
    IEnumerator timer(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
