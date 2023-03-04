using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class activateAbility : MonoBehaviour
{
    [SerializeField] List<abilities> abilityBar = new List<abilities>();
    float cooldownTime;
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
        if (gameManager.instance.playerScript.fireOn || gameManager.instance.playerScript.iceOn)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, 10))
            {
                hit.collider.GetComponent<IDamage>().takeDamage(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q) && gameManager.instance.AbilityOneS.wasSpellUsed())
        {
            abilityTexture = abilityOne.GetComponent<Image>().sprite;
            if (abilityOne.GetComponent<Image>().sprite.name != "None2")
            {
                abilityActivation(abilityTexture);
                foreach (abilities stats in abilityBar)
                {
                    if (stats.abilityImage == abilityTexture)
                    {
                        gameManager.instance.AbilityOneS.wasSpellUsed();
                        gameManager.instance.AbilityOneS.coolDownStart(stats.cooldownTime);
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.R) && gameManager.instance.AbilityTwoS.wasSpellUsed())
        {
            abilityTexture = abilityTwo.GetComponent<Image>().sprite;
            if (abilityTexture.name == "None2")
            {
                return;
            }
            abilityActivation(abilityTexture);
            foreach (abilities stats in abilityBar)
            {
                if (stats.abilityImage == abilityTexture)
                {
                    Debug.Log("cooldownTime: " + stats.cooldownTime);
                    gameManager.instance.AbilityTwoS.wasSpellUsed();
                    gameManager.instance.AbilityTwoS.coolDownStart(stats.cooldownTime);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.F) && gameManager.instance.AbilityThreeS.wasSpellUsed())
        {
            abilityTexture = abilityThree.GetComponent<Image>().sprite;
            if (abilityTexture.name == "None2")
            {
                return;
            }
            abilityActivation(abilityTexture);
            foreach (abilities stats in abilityBar)
            {
                if (stats.abilityImage == abilityTexture)
                {
                    Debug.Log("cooldownTime: " + stats.cooldownTime);
                    gameManager.instance.AbilityThreeS.wasSpellUsed();
                    gameManager.instance.AbilityThreeS.coolDownStart(stats.cooldownTime);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && gameManager.instance.AbilityFourS.wasSpellUsed())
        {
            abilityTexture = abilityFour.GetComponent<Image>().sprite;
            if (abilityTexture.name == "None2")
            {
                return;
            }
            abilityActivation(abilityTexture);
            foreach (abilities stats in abilityBar)
            {
                if (stats.abilityImage == abilityTexture)
                {
                    gameManager.instance.AbilityFourS.wasSpellUsed();
                    gameManager.instance.AbilityFourS.coolDownStart(stats.cooldownTime);
                }
            }
        }
    }
    public void abilityActivation(Sprite abilityTexture)
    {
        foreach (abilities stats in abilityBar)
        {
            if (stats.abilityImage == abilityTexture)
            {
                if (stats.abilityName == "Plasma Grenade")
                {
                    gameManager.instance.playerScript.throwGrenade();
                }
                else if (stats.abilityName == "Shield")
                {
                    abilityAudio = stats.abilityAudio;
                    abilityAudioVol = stats.abilityAudioVol;
                    gameManager.instance.aud.PlayOneShot(abilityAudio, abilityAudioVol);
                    StartCoroutine(abilityCoolShield(cooldownTime));
                }
                else if (stats.abilityName == "Fire")
                {
                    abilityAudio = stats.abilityAudio;
                    abilityAudioVol = stats.abilityAudioVol;
                    gameManager.instance.aud.PlayOneShot(abilityAudio, abilityAudioVol);
                    StartCoroutine(abilityCoolFire(3));
                }
                else if (stats.abilityName == "Ice")
                {
                    abilityAudio = stats.abilityAudio;
                    abilityAudioVol = stats.abilityAudioVol;
                    gameManager.instance.aud.PlayOneShot(abilityAudio, abilityAudioVol);
                    StartCoroutine(abilityCoolIce(3));
                }
                else if (stats.abilityName == "Swarm")
                {
                    abilityAudio = stats.abilityAudio;
                    abilityAudioVol = stats.abilityAudioVol;
                    gameManager.instance.playerScript.swarm();
                }
            }
        }
    }
    public void abilityPickup(abilities stats)
    {
        if (abilityBar.Count < 4)
        {
            cooldownTime = stats.cooldownTime;
            abilityImage = stats.abilityImage;
            abilityAudio = stats.abilityAudio;
            abilityAudioVol = stats.abilityAudioVol;
            abilityName = stats.abilityName;
            abilityInfo = stats.abilityInfo;

            abilityBar.Add(stats);

            gameManager.instance.displayAbility(abilityInfo);

            if (abilityOne.GetComponent<Image>().sprite.name == "None2")
            {
                abilityOne.GetComponent<Image>().sprite = abilityImage;
            }
            else if (abilityTwo.GetComponent<Image>().sprite.name == "None2")
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
    public IEnumerator abilityCoolShield(float cooldown)
    {
        gameManager.instance.playerScript.shieldOnPlayer.GetComponent<shield>().shieldStart();
        yield return new WaitForSeconds(cooldown);
        if (gameManager.instance.shieldOn)
        {
            gameManager.instance.playerScript.shieldOnPlayer.GetComponent<shield>().shutOffShield();
        }
    }
    public IEnumerator abilityCoolFire(float cooldown)
    {
        gameManager.instance.playerScript.fireOnPlayer.SetActive(true);
        gameManager.instance.playerScript.fireOn = true;        
        yield return new WaitForSeconds(cooldown);
        gameManager.instance.playerScript.fireOnPlayer.SetActive(false);
        gameManager.instance.playerScript.fireOn = false;
    }
    public IEnumerator abilityCoolIce(float cooldown)
    {
        gameManager.instance.playerScript.iceOnPlayer.SetActive(true);
        gameManager.instance.playerScript.iceOn = true;
        yield return new WaitForSeconds(cooldown);
        gameManager.instance.playerScript.iceOnPlayer.SetActive(false);
        gameManager.instance.playerScript.iceOn = false;
    }
}
