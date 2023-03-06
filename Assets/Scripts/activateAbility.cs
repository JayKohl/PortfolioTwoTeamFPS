using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class activateAbility : MonoBehaviour
{
    [SerializeField] public List<abilities> abilityBar = new List<abilities>();
    [SerializeField] public List<abilities> abilitiesInventory = new List<abilities>();
    float cooldownTime;
    Sprite abilityImage;
    AudioClip abilityAudio;
    float abilityAudioVol;
    Sprite abilityInfo;
    Sprite abilityName;

    GameObject abilityOne;
    GameObject abilityTwo;
    GameObject abilityThree;
    GameObject abilityFour;
    Sprite abilityTexture;

    Image inventorySlot1;
    Image inventorySlot2;
    Image inventorySlot3;
    Image inventorySlot4;
    Image inventorySlot5;
    Image inventorySlot6;

    private void Start()
    {
        abilityOne = gameManager.instance.AbilityOne;
        abilityTwo = gameManager.instance.AbilityTwo;
        abilityThree = gameManager.instance.AbilityThree;
        abilityFour = gameManager.instance.AbilityFour;
        setupAbilities();
    }
    void Update()
    {
        if (gameManager.instance.playerScript.fireOn || gameManager.instance.playerScript.iceOn)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, 10))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    hit.collider.GetComponent<IDamage>().takeDamage(0);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(abilityBar.Count < 1) { return; }
            if (gameManager.instance.AbilityOneS.wasSpellUsed())
            {
                abilityTexture = abilityOne.GetComponent<Image>().sprite;
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
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (abilityBar.Count < 2) { return; }
            if (gameManager.instance.AbilityTwoS.wasSpellUsed())
            {
                abilityTexture = abilityTwo.GetComponent<Image>().sprite;
                abilityActivation(abilityTexture);
                foreach (abilities stats in abilityBar)
                {
                    if (stats.abilityImage == abilityTexture)
                    {
                        gameManager.instance.AbilityTwoS.wasSpellUsed();
                        gameManager.instance.AbilityTwoS.coolDownStart(stats.cooldownTime);
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            if (abilityBar.Count < 3) { return; }
            if (gameManager.instance.AbilityThreeS.wasSpellUsed())
            {
                abilityTexture = abilityThree.GetComponent<Image>().sprite;
                abilityActivation(abilityTexture);
                foreach (abilities stats in abilityBar)
                {
                    if (stats.abilityImage == abilityTexture)
                    {
                        gameManager.instance.AbilityThreeS.wasSpellUsed();
                        gameManager.instance.AbilityThreeS.coolDownStart(stats.cooldownTime);
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (abilityBar.Count < 4) { return; }
            if (gameManager.instance.AbilityFourS.wasSpellUsed())
            {
                abilityTexture = abilityFour.GetComponent<Image>().sprite;
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
        cooldownTime = stats.cooldownTime;
        abilityImage = stats.abilityImage;
        abilityAudio = stats.abilityAudio;
        abilityAudioVol = stats.abilityAudioVol;
        abilityInfo = stats.abilityInfo;

        if (abilityBar.Count < 4)
        {
            for(int i = 0; i < abilityBar.Count; i++)
            {
                if(stats == abilityBar[i])
                {
                    gameManager.instance.displayAbility(abilityInfo);
                    return;
                }
            }
            abilityBar.Add(stats);
            abilitiesInventory.Add(stats);

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
            for (int i = 0; i < abilitiesInventory.Count; i++)
            {
                if (stats == abilitiesInventory[i])
                {
                    gameManager.instance.displayAbility(abilityInfo);
                    return;
                }
            }
            abilitiesInventory.Add(stats);
            gameManager.instance.displayAbility(abilityInfo);

            //do Else If statements similar to lines 184 - 198
        }
    }
    public void replaceAbility(int num, Sprite abilityTexture)
    {
        foreach (abilities stats in abilitiesInventory)
        {
            if (stats.abilityImage == abilityTexture)
            {
                abilityBar[num].abilityName = stats.abilityName;
                abilityBar[num].cooldownTime = stats.cooldownTime;
                abilityBar[num].abilityImage = stats.abilityImage;
                abilityBar[num].abilityAudio = stats.abilityAudio;
                abilityBar[num].abilityAudioVol = stats.abilityAudioVol;
                abilityBar[num].abilityInfo = stats.abilityInfo;                
            }
        }
    }
    public void setupAbilities()
    {
        for (int i = 0; i < abilityBar.Count; i++)
        {
            abilityImage = abilityBar[i].abilityImage;

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
