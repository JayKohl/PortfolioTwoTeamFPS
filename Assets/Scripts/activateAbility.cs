using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class activateAbility : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Sprite abilityTexture = gameManager.instance.AbilityOne.GetComponent<Image>().sprite;
            abilityActivation(abilityTexture);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Sprite abilityTexture = gameManager.instance.AbilityTwo.GetComponent<Image>().sprite;
            abilityActivation(abilityTexture);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Sprite abilityTexture = gameManager.instance.AbilityThree.GetComponent<Image>().sprite;
            abilityActivation(abilityTexture);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Sprite abilityTexture = gameManager.instance.AbilityFour.GetComponent<Image>().sprite;
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
}
