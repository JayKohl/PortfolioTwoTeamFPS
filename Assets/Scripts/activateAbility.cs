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
    float abilityAudioVol;
    string abilityName;
    Sprite abilityInfo;

    GameObject abilityOne;
    GameObject abilityTwo;
    GameObject abilityThree;
    GameObject abilityFour;
    Sprite abilityTexture;

    private void Start()
    {
        abilityOne = gameManager.instance.AbilityOne;
        abilityTwo = gameManager.instance.AbilityTwo;
        abilityThree = gameManager.instance.AbilityThree;
        abilityFour = gameManager.instance.AbilityFour;
    }
    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            abilityTexture = abilityOne.GetComponent<Image>().sprite;
            abilityActivation(abilityTexture);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            abilityTexture = abilityTwo.GetComponent<Image>().sprite;
            abilityActivation(abilityTexture);            
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            abilityTexture = abilityThree.GetComponent<Image>().sprite;
            abilityActivation(abilityTexture);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            abilityTexture = abilityFour.GetComponent<Image>().sprite;
            abilityActivation(abilityTexture);
        }
    }
    public void abilityActivation(Sprite abilityTexture)
    {
        foreach (abilities stats in abilityBar)
        {
            if(stats.abilityImage == abilityTexture)
            {
                if (stats.abilityName == "Plasma Grenade")
                {
                    cooldownTime = stats.cooldownTime;
                    cooldownTimer = stats.cooldownTimer;
                    gameManager.instance.playerScript.throwGrenade();
                }
                else if (stats.abilityName == "Shield")
                {
                    cooldownTime = stats.cooldownTime;
                    cooldownTimer = stats.cooldownTimer;
                    abilityAudio = stats.abilityAudio;
                    abilityAudioVol = stats.abilityAudioVol;
                    gameManager.instance.aud.PlayOneShot(abilityAudio, abilityAudioVol);

                    StartCoroutine(gameManager.instance.playerScript.abilityCoolShield(cooldownTime));
                }
            }            
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
            abilityAudioVol = stats.abilityAudioVol;
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
