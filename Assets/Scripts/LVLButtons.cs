using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LVLButtons : MonoBehaviour
{
    
    [SerializeField] public GameObject healthcover;
    [SerializeField] public GameObject speedcover;
    [SerializeField] public GameObject damagecover;
    [SerializeField] public GameObject jumpcover;
    [SerializeField] public GameObject defensecover;
    [SerializeField] public GameObject abilitycover;
    [SerializeField] public GameObject XPcover;
    [SerializeField] public GameObject cooldowncover;
    //[SerializeField] public GameObject shortCoin;

    [SerializeField] public GameObject errorText;
    [SerializeField] public int tokensum;
    [SerializeField] public bool coolDownReduced = false;
    


    void Update()
    {
        if (tokensum != gameManager.instance.lvlscript.GetTokens())
        {
            tokensum = gameManager.instance.lvlscript.GetTokens();
        }
    }

    public void HealthUp()
    {
        if ((tokensum - 2) >= 0)
        {
            gameManager.instance.playerScript.HP = gameManager.instance.playerScript.HP * 2;
            gameManager.instance.playerScript.hpOriginal = gameManager.instance.playerScript.HP;
            healthcover.SetActive(true);
            gameManager.instance.lvlscript.DecrementTokens(2);
        }
        else
        {
            StartCoroutine(ErrorText());
        }

    }

    public void DamageUp()
    {
        if ((tokensum - 3) >= 0)
        {
            gameManager.instance.playerScript.weaponDamageMulti = 2;
            gameManager.instance.playerScript.shootDamage = gameManager.instance.playerScript.shootDamage * 2;
            damagecover.SetActive(true);
            gameManager.instance.lvlscript.DecrementTokens(3);
        }
        else
        {
            StartCoroutine(ErrorText());
        }
    }

    public void SpeedUp()
    {
        if ((tokensum - 1) >= 0)
        {
            gameManager.instance.playerScript.speedOriginal = gameManager.instance.playerScript.playerSpeed + (gameManager.instance.playerScript.playerSpeed * 0.5f);
            gameManager.instance.playerScript.playerSpeed = gameManager.instance.playerScript.playerSpeed + (gameManager.instance.playerScript.playerSpeed * 0.5f);
            gameManager.instance.playerScript.runSpeed = gameManager.instance.playerScript.runSpeed + (gameManager.instance.playerScript.runSpeed * 0.5f);
            speedcover.SetActive(true);
            gameManager.instance.lvlscript.DecrementTokens(1);
        }
        else
        {
            StartCoroutine(ErrorText());
        }
    }

    public void JumpUp()
    {
        if ((tokensum - 3) >= 0)
        {
            gameManager.instance.playerScript.jumpTimes = gameManager.instance.playerScript.jumpTimes + 1;
            jumpcover.SetActive(true);
            gameManager.instance.lvlscript.DecrementTokens(3);
        }
        else
        {
            StartCoroutine(ErrorText());
        }
    }

    public void DefenseUp()
    {
        if ((tokensum - 3) >= 0)
        {
            gameManager.instance.playerScript.dmgDivide = 2;
            defensecover.SetActive(true);
            gameManager.instance.lvlscript.DecrementTokens(3);
        }
        else
        {
            StartCoroutine(ErrorText());
        }
    }
    public void AbilityAttackUp() // not finished
    {
        if ((tokensum - 5) >= 0)
        {

            abilitycover.SetActive(true);
            gameManager.instance.lvlscript.DecrementTokens(5);
        }
        else
        {
            StartCoroutine(ErrorText());
        }
    }

    public void XPUp()  //not finished
    {
        if ((tokensum - 5) >= 0)
        {
            gameManager.instance.lvlscript.XPMod = 1.8f;
            XPcover.SetActive(true);
            gameManager.instance.lvlscript.DecrementTokens(5);
        }
        else
        {
            StartCoroutine(ErrorText());
        }
    }

    public void CooldownDown() //not finisheds
    {
        if ((tokensum - 3) >= 0)
        {
            coolDownReduced = true;
            cooldowncover.SetActive(true);
            gameManager.instance.lvlscript.DecrementTokens(3);
        }
        else
        {
            StartCoroutine(ErrorText());
        }
    }

    IEnumerator ErrorText()
    {
        errorText.SetActive(true);
        yield return new WaitForSecondsRealtime(1.6f);
        errorText.SetActive(false);
    }
}
