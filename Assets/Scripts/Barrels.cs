using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrels : MonoBehaviour, IDamage
{
    [SerializeField] int barrilHP;
    [SerializeField] GameObject explotion;

    // Start is called before the first frame update

    private void Start()
    {
     
        barrilHP = 20;
    }
    public virtual void takeDamage(int damage)
    {
        barrilHP -= damage;

        if(barrilHP <= 0)
        {
            Instantiate(explotion, transform.position, transform.rotation);
          
            Destroy(gameObject);

        }
    }
    
}
