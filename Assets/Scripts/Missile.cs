using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("----- Missile Info -----")]    
    [SerializeField] int maxTravelTime;
    [SerializeField] GameObject explosion;
    [SerializeField] int detonationTimer;
    Vector3 playerDirection;
    float angleToPlayer;
    
    void Start()
    {
        playerDirection = (gameManager.instance.player.transform.position - transform.position).normalized;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);
        StartCoroutine(timer());        
    }
    IEnumerator timer()
    {
        yield return new WaitForSeconds(detonationTimer);
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
