using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushBack : MonoBehaviour
{
    [SerializeField] public int pushBackDamage;
    [SerializeField] public int pushBackDistance;
    [SerializeField] public float activeTime;

    bool active;

    void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            if (other.CompareTag("Enemy") || other.CompareTag("EnemyBoss"))
            {
                other.GetComponent<enemyAI>().takeDamage(pushBackDamage);
                //other.GetComponent<enemyAI>().pushBackDir();
                //gameManager.instance.playerScript.pushbackDir((gameManager.instance.player.transform.position - transform.position).normalized * pushBackDistance);
            }
        }
    }
    public void startPush()
    {
        StartCoroutine(pushBackStart());
    }
    public IEnumerator pushBackStart()
    {
        active = true;
        yield return new WaitForSeconds(activeTime);
        active = false;
    }
}
