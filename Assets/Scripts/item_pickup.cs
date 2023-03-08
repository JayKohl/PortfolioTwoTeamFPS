using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_pickup : MonoBehaviour
{
    [Header("Item Type")]
    [SerializeField] bool isHealth;
    [SerializeField] bool isWeapon;

    [Header("Health Pack")]
    [SerializeField] int hpAmount;    

    [Header("Weapon Stats")]
    [SerializeField] weaponStats weapon;

    private void Start()
    {
        StartCoroutine(rotate());
    }
    private IEnumerator rotate()
    {
        while (true)
        {
            if (!gameManager.instance.isPaused)
            {
                transform.Rotate(0f, 0.5f, 0f, Space.Self);
            }
            yield return new WaitForSeconds(1f / 60);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isWeapon)
            {                
                gameManager.instance.playerScript.weaponPickup(weapon);
                Destroy(gameObject);
            }
            else if (isHealth && gameManager.instance.playerScript.HP < gameManager.instance.playerScript.hpOriginal)
            {                
                gameManager.instance.playerScript.giveHP(hpAmount);
                Destroy(gameObject);
            }            
        }
    }    
}
