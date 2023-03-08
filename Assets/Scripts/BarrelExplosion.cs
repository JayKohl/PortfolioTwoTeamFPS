using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelExplosion : MonoBehaviour
{
    bool playerIn;
    [SerializeField] public int pushBackDamageEX;
    private void Start()
    {
        StartCoroutine(disappear());
    }
    IEnumerator disappear()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerIn)
        {
            playerIn = true;
            gameManager.instance.playerScript.takeDamage(25);
            gameManager.instance.playerScript.pushbackDir((gameManager.instance.player.transform.position - transform.position).normalized * pushBackDamageEX);
        }
        else if (other.CompareTag("EnemyAI"))
        {
            other.gameObject.GetComponent<enemyAI>().takeDamage(25);
        }
        else if (other.CompareTag("EnemyBoss"))
        {
            other.gameObject.GetComponent<enemyBossAI>().takeDamage(25);
        }
        else if (other.CompareTag("Turret"))
        {
            other.gameObject.GetComponent<enemyTurretAI>().takeDamage(25);
        }
    }
}