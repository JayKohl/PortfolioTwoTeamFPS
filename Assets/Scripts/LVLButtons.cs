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
    [SerializeField] public GameObject defencecover;
    [SerializeField] public GameObject abilitycover;
    [SerializeField] public GameObject XPcover;
    [SerializeField] public GameObject cooldowncover;
    [SerializeField] public GameObject shortCoin;
    public LevelSystem lvlscript;
    [SerializeField] public int tokensum;

    
    private void Start()
    {
        //lvlscript = gameManager.instance.lvlscript;
    }

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
        
    }

    public void JumpUp()
    {

    }

    public void DefenseUp()
    {

    }
    public void AbilityAttackUp()
    {

    }

    public void XPUp()
    {

    }

    public void CooldownDown()
    {

    }
}
