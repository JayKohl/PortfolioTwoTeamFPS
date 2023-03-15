using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrels : MonoBehaviour, IDamage
{
    [SerializeField] int barrilHP;
    [SerializeField] GameObject explotion;
    public int damage;
    public bool alive;
    // Start is called before the first frame update

    private void Start()
    {
        alive = true;
        barrilHP = 20;
    }
    public virtual void takeDamage(int damage)
    {
        barrilHP -= damage;

        if(barrilHP <= 0 && alive)
        {
            Instantiate(explotion, transform.position, transform.rotation);
            alive = false;
            Destroy(gameObject);

        }
    }
    
}
