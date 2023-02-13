using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class item_pickup : MonoBehaviour
{
    float time = 1;

    [Header("Item Type")]
    [SerializeField] bool isHealth;
    [SerializeField] bool isWeapon;

    [Header("Health Pack")]
    [SerializeField] int hpAmount;    

    [Header("Weapon Stats")]
    [SerializeField] weaponStats weapon;

    void Update()
    {
        StartCoroutine(bounce());
    }
    IEnumerator bounce()
    {
        if (!gameManager.instance.isPaused)
        {
            transform.Translate(0, 0.002f, 0, Space.World);
            yield return new WaitForSeconds(time);
            transform.Translate(0, -0.002f, 0, Space.World);
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
