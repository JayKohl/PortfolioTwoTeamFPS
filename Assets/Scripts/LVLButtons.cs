using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVLButtons : MonoBehaviour
{
    public void AttackUp()
    {
        
    }

    public void DamageUp()
    {
        
    }

    public void SpeedUp()
    {
        gameManager.instance.playerScript.speedOriginal = gameManager.instance.playerScript.playerSpeed + (gameManager.instance.playerScript.playerSpeed * 0.3f);
        gameManager.instance.playerScript.playerSpeed = gameManager.instance.playerScript.playerSpeed + (gameManager.instance.playerScript.playerSpeed * 0.3f);
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
