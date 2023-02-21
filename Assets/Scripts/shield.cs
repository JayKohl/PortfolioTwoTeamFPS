using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield : MonoBehaviour
{
    int shieldHP;
    [SerializeField] int shieldOrig;
    [SerializeField] float cooldown;

    void Start()
    {
        shieldHP = shieldOrig;
    }
    public void shieldStart()
    {
        gameManager.instance.shieldOn = true;
        gameObject.SetActive(true);
    }
    public void shieldTakeDamage(int dmg)
    {
        shieldHP -= dmg;
        if (shieldHP <= 0)
        {
            gameManager.instance.playerScript.StartCoroutine(gameManager.instance.playerScript.abilityCoolShield(cooldown));
            gameManager.instance.playerScript.shieldOffPlayer();
            gameManager.instance.shieldCoolDown();
            gameObject.SetActive(false);
            gameManager.instance.shieldOn = false;
        }
    }
}
