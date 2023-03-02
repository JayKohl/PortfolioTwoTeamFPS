using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class activateAbility : MonoBehaviour
{
    [SerializeField] List<abilities> abilityBar = new List<abilities>();
    float cooldownTime;
    float cooldownTimer;
    Sprite abilityImage;
    AudioClip abilityAudio;
    string abilityName;
    Sprite abilityInfo;

    GameObject abilityOne;
    GameObject abilityTwo;
    GameObject abilityThree;
    GameObject abilityFour;

    private void Start()
    {
        abilityOne = gameManager.instance.AbilityOne;
        abilityTwo = gameManager.instance.AbilityTwo;
        abilityThree = gameManager.instance.AbilityThree;
        abilityFour = gameManager.instance.AbilityFour;
    }
    void Update()
    {
        Sprite abilityTexture = null;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            abilityTexture = abilityOne.GetComponent<Image>().sprite;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            abilityTexture = abilityTwo.GetComponent<Image>().sprite;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            abilityTexture = abilityThree.GetComponent<Image>().sprite;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            abilityTexture = abilityFour.GetComponent<Image>().sprite;            
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

            if(abilityOne.GetComponent<Image>().sprite.name == "None2")
            {
                abilityOne.GetComponent<Image>().sprite = abilityImage;
            }
            else if(abilityTwo.GetComponent<Image>().sprite.name == "None2")
            {
                abilityTwo.GetComponent<Image>().sprite = abilityImage;
            }
            else if (abilityThree.GetComponent<Image>().sprite.name == "None2")
            {
                abilityThree.GetComponent<Image>().sprite = abilityImage;
            }
            else if (abilityFour.GetComponent<Image>().sprite.name == "None2")
            {
                abilityFour.GetComponent<Image>().sprite = abilityImage;
            }
        }
        else
        {
            //replace ability window
            //OR
            //send new ability to inventory to replace later
        }
    }
}
