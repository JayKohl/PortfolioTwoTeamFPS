using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionGrenade : MonoBehaviour
{
    [SerializeField] public int grenadeDamage;
    [SerializeField] int pushBackDistance;
    bool playerIn;
    private void Start()
    {
        StartCoroutine(timer(1f));
    }
    private void OnTriggerEnter(Collider other)
    {
        //if(other.CompareTag("Enemy") || other.CompareTag("EnemyBoss"))
        {
            other.GetComponent<Collider>().GetComponent<IDamage>().takeDamage(grenadeDamage);
        }
        //gameManager.instance.playerScript.pushbackDir((gameManager.instance.player.transform.position - transform.position).normalized * pushBackDistance);
    }
    IEnumerator timer(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
