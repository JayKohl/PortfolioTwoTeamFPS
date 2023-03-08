using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletSentry : MonoBehaviour
{
    [Header("----- Bullet Info -----")]
    public int bulletDamage;
    [SerializeField] int maxTravelTime;
    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, maxTravelTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        target = other.gameObject;
        if (other.CompareTag("Enemy"))
        {            
            target.GetComponent<enemyAI>().takeDamage(bulletDamage);
        }
        else if(other.CompareTag("EnemyBoss"))
        {
            target.GetComponent<enemyBossAI>().takeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}
