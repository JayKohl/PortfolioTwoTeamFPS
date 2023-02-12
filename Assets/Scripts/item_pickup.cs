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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isHealth && gameManager.instance.playerScript.HP < gameManager.instance.playerScript.hpOriginal)
            {
                gameManager.instance.playerScript.giveHP(hpAmount);
                Destroy(gameObject);
            }
            else if(isWeapon)
            {
                gameManager.instance.playerScript.weaponPickup(weapon);
                Destroy(gameObject);
            }            
        }
    }    
}
