using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield : MonoBehaviour
{
    public int shieldHP = 5;
    [SerializeField] int shieldOrig;
    [SerializeField] float cooldown;

    void Start()
    {
       
        shieldOrig = shieldHP;
    }
    public void shieldStart()
    {
               
        gameObject.SetActive(true);
    }
    public void shieldTakeDamage(int dmg)
    {
        shieldHP -= dmg;
        if (shieldHP <= 0)
        {

            gameManager.instance.shieldUI.SetActive(false);
            //gameManager.instance.shieldCoolDown();
            gameObject.SetActive(false);
            gameManager.instance.shieldOn = false;
            shieldHP = shieldOrig;

        }
    }
    public void timeOut()
    {
        gameManager.instance.shieldUI.SetActive(false);
        //gameManager.instance.shieldCoolDown();
        gameObject.SetActive(false);
        gameManager.instance.shieldOn = false;
        shieldHP = shieldOrig;
    }
    public float GetCoolDown()
    {
        return cooldown;
    }
}
