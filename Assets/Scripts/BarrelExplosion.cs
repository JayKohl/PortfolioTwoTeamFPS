using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelExplosion : MonoBehaviour
{
    
    bool playerIn;
    [SerializeField]  int range;
    [SerializeField] int damage;
    //bool exploded;
    private void Start()
    {
        
        StartCoroutine(disappear());
    }
    IEnumerator disappear()
    {
        //damage = GetComponentInParent<Barrels>().damage;
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
    public void OnTriggerEnter(Collider other)
    {
        float distance = Vector3.Distance(other.transform.position, transform.position);
        if (distance <= range)
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
}