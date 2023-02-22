using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield : MonoBehaviour
{
    int shieldHP;
    [SerializeField] int shieldOrig;
    [SerializeField] float cooldown;
    [SerializeField] public int shielTimer;

    void Start()
    {
        shieldHP = shieldOrig;
    }
    
    public void shieldStart()
    {
        gameManager.instance.shieldUI.SetActive(true);
        gameManager.instance.shieldOn = true;
        gameObject.SetActive(true);
    }
    public void shieldTakeDamage(int dmg)
    {
        shieldHP -= dmg;
        if (shieldHP <= 0)
        {
            timeOver();
            ////gameManager.instance.playerScript.shieldOffPlayer();
            ////gameManager.instance.shieldCoolDown();
            //gameObject.SetActive(false);
            //gameManager.instance.shieldOn = false;
            //shieldHP = shieldOrig;
        }
    }
    public void timeOver()
    {
        //gameManager.instance.playerScript.shieldOffPlayer();
        gameManager.instance.AbilityTwoS.wasSpellUsed();
        gameManager.instance.AbilityTwoS.coolDownAbility();
        gameObject.SetActive(false);
        gameManager.instance.shieldOn = false;
        gameManager.instance.shieldUI.SetActive(false);
        shieldHP = shieldOrig;

    }    
}
