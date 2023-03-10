using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [Header("----- Bullet Info -----")]
    public int bulletDamage;
    [SerializeField] int maxTravelTime;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, maxTravelTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.takeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}
