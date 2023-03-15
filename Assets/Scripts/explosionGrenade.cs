using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionGrenade : MonoBehaviour
{
    [SerializeField] public int grenadeDamage;
    [SerializeField] int time;

    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip explosion;

    void Start()
    {        
        StartCoroutine(timer(time));
    }
    void OnTriggerEnter(Collider other)
    {
        float distance = Vector3.Distance(other.transform.position, transform.position);
        if(distance <= 15)
        {
            if (gameManager.instance.lvlbuttons.abilityDamageUp)
            {
                grenadeDamage += 3;
            }
            if (other.CompareTag("Enemy") || other.CompareTag("EnemyBoss") || other.CompareTag("Turret"))
            {
                other.gameObject.GetComponent<enemyAI>().takeDamage(grenadeDamage);
                aud.PlayOneShot(explosion, gameManager.instance.soundVol);
            }
        }
    }
    IEnumerator timer(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
