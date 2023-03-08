using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LVLButtons : MonoBehaviour
{
    public void HealthUp()
    {
        gameManager.instance.playerScript.HP = gameManager.instance.playerScript.HP * 2;
        gameManager.instance.playerScript.hpOriginal = gameManager.instance.playerScript.HP;
    }

    public void DamageUp()
    {
        
    }

    public void SpeedUp()
    {
        gameManager.instance.playerScript.speedOriginal = gameManager.instance.playerScript.playerSpeed + (gameManager.instance.playerScript.playerSpeed * 0.3f);
        gameManager.instance.playerScript.playerSpeed = gameManager.instance.playerScript.playerSpeed + (gameManager.instance.playerScript.playerSpeed * 0.3f);
        gameManager.instance.playerScript.runSpeed = gameManager.instance.playerScript.runSpeed + (gameManager.instance.playerScript.runSpeed * 0.3f);
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
