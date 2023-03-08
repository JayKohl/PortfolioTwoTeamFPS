using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBarrel : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] AudioSource aud;
    [SerializeField] public int pushBackDamageGrenade;
    [SerializeField] GameObject BarrelExplosion;
    [Range(1, 5)] [SerializeField] int HP;

    [Header("----- Sound -----")]
    [SerializeField] AudioClip[] audBarrelExplode;
    [Range(0, 1)] [SerializeField] float audBarrelExplodeVol;
    bool playerIn;


    public void takeDamage(int dmg)
    {
        HP -= dmg;

        if (HP <= 0)
        {
            //aud.PlayOneShot(audBarrelExplode[Random.Range(0, audBarrelExplode.Length)], audBarrelExplodeVol);
            Destroy(gameObject);
            Instantiate(BarrelExplosion, transform.position, BarrelExplosion.transform.rotation);
        }
        else
        {
            StartCoroutine(flashBreakDamage());
        }
    }

    public void OnTriggerEnter(Collider other)
    {
         void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !playerIn)
            {
                playerIn = true;
                gameManager.instance.playerScript.takeDamage(25);
                gameManager.instance.playerScript.pushbackDir((gameManager.instance.player.transform.position - transform.position).normalized * pushBackDamageGrenade);
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

    IEnumerator flashBreakDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        model.material.color = Color.white;
    }
}
