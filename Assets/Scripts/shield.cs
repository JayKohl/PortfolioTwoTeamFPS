using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shield : MonoBehaviour
{
    public int shieldHP = 5;
    [SerializeField] int shieldOrig;
    [SerializeField] float cooldown;
    Color thisColor = Color.red;
    Color originalColor = Color.red;
    void Start()
    {
        thisColor.a = 0.20f;
        originalColor.a = gameManager.instance.shieldUI.GetComponent<Image>().color.a;
        originalColor.g = gameManager.instance.shieldUI.GetComponent<Image>().color.g;
        originalColor.b = gameManager.instance.shieldUI.GetComponent<Image>().color.b;
        originalColor.r = gameManager.instance.shieldUI.GetComponent<Image>().color.r;
        thisColor.a = 1;
        shieldOrig = shieldHP;
    }
    public void shieldStart()
    {
               
        gameObject.SetActive(true);
    }
    public void shieldTakeDamage(int dmg)
    {
        shieldHP -= dmg;
        StartCoroutine(flashShield());
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

    IEnumerator flashShield()
    {
        gameManager.instance.shieldUI.GetComponent<Image>().color = originalColor;
        yield return new WaitForSeconds(.1f);
        gameManager.instance.shieldUI.GetComponent<Image>().color = thisColor;
    }
}
