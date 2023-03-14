using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelExplosion : MonoBehaviour
{
    
    bool playerIn;
    int damage;
    private void Start()
    {
        GetComponentInParent<Barrels>();
        StartCoroutine(disappear());
        
    }
    IEnumerator disappear()
    {
        yield return new WaitForSeconds(2);
        Destroy(this, .1f);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamage>() != null)
        {
            other.GetComponent<IDamage>().takeDamage(damage);
        }
        else if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.takeDamage(damage);
        }
    }
}