using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class activateAbility : MonoBehaviour
{
    List<abilities> abilityBar = new List<abilities>();
    float cooldownTime;
    float cooldownTimer;
    Sprite abilityImage;
    AudioClip abilityAudio;
    string abilityName;
    Sprite abilityInfo;
    void Update()
    {
        Sprite abilityTexture = null;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            abilityTexture = gameManager.instance.AbilityOne.GetComponent<Image>().sprite;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            abilityTexture = gameManager.instance.AbilityTwo.GetComponent<Image>().sprite;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            abilityTexture = gameManager.instance.AbilityThree.GetComponent<Image>().sprite;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            abilityTexture = gameManager.instance.AbilityFour.GetComponent<Image>().sprite;            
        }
        if(abilityTexture != null)
        {
            abilityActivation(abilityTexture);
        }
    }
    public void abilityActivation(Sprite abilityTexture)
    {
        if(abilityTexture.name == "grenade")
        {
            gameManager.instance.playerScript.throwGrenade();
        }
    }
    public void abilityPickup(abilities stats)
    {
        if (abilityBar.Count < 4)
        {
            cooldownTime = stats.cooldownTime;
            cooldownTimer = stats.cooldownTimer;
            abilityImage = stats.abilityImage;
            abilityAudio = stats.abilityAudio;
            abilityName = stats.abilityName;
            abilityInfo = stats.abilityInfo;
            abilityBar.Add(stats);
            gameManager.instance.displayAbility(abilityInfo);
        }
        else
        {
            //replace ability window
        }
    }
}
