using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class activateAbility : MonoBehaviour
{
    [SerializeField] public List<abilities> abilityBar = new List<abilities>();
    [SerializeField] List<Sprite> hackAnimation = new List<Sprite>();
    float cooldownTime;
    Sprite abilityImage;
    AudioClip abilityAudio;
    float abilityAudioVol;
    Sprite abilityInfo;
    string abilityName;

    GameObject abilityOne;
    GameObject abilityTwo;
    GameObject abilityThree;
    GameObject abilityFour;
    Sprite abilityTexture;

    GameObject inventorySlot1;
    GameObject inventorySlot2;
    GameObject inventorySlot3;
    GameObject inventorySlot4;
    GameObject inventorySlot5;
    GameObject inventorySlot6;

    TextMeshProUGUI inventoryMessageUpdate;
    string defaultMsg = "Drag and drop abilities from your Inventory window to your Abilities Bar.";
    string pickUpMsg_New = "You have found a new ability!";
    string pickUpMsg_Inventory = "A new ability was added to your inventory! Press {I} to open your Inventory.";
    string pickUpMsg_Own = "You already own this ability.";

    public bool inventoryScreenOn;
    int hackCounter;
    private GameObject hackTarget;
    bool cancelHack;

    private void Start()
    {
        abilityOne = gameManager.instance.AbilityOne;
        abilityTwo = gameManager.instance.AbilityTwo;
        abilityThree = gameManager.instance.AbilityThree;
        abilityFour = gameManager.instance.AbilityFour;

        inventorySlot1 = gameManager.instance.inventorySlot1;
        inventorySlot2 = gameManager.instance.inventorySlot2;
        inventorySlot3 = gameManager.instance.inventorySlot3;
        inventorySlot4 = gameManager.instance.inventorySlot4;
        inventorySlot5 = gameManager.instance.inventorySlot5;
        inventorySlot6 = gameManager.instance.inventorySlot6;

        inventoryMessageUpdate = gameManager.instance.inventoryMessages;

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
            if (abilityBar.Count < 1) { return; }
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
        else if (Input.GetKeyDown(KeyCode.I))
        {
            if(inventoryScreenOn)
            {
                gameManager.instance.inventoryMessageBox.SetActive(false);
                inventoryScreenOn = false;
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                gameManager.instance.inventory.SetActive(false);
            }
            else
            {
                inventoryScreenOn = true;
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                inventoryMessageUpdate.text = defaultMsg;
                gameManager.instance.inventoryMessageBox.SetActive(true);
                gameManager.instance.inventory.SetActive(true);
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
                else if(stats.abilityName == "Hack")
                {
                    abilityAudio = stats.abilityAudio;
                    abilityAudioVol = stats.abilityAudioVol;
                    gameManager.instance.aud.PlayOneShot(abilityAudio, abilityAudioVol);
                    hack();
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
        abilityName = stats.abilityName;

        if (abilityBar.Count < 4)
        {
            for(int i = 0; i < abilityBar.Count; i++)
            {
                if(stats == abilityBar[i])
                {
                    gameManager.instance.displayAbility(abilityInfo);
                    inventoryMessageUpdate.text = pickUpMsg_Own;
                    gameManager.instance.inventoryMessageBox.SetActive(true);
                    return;
                }
            }
            abilityBar.Add(stats);
            gameManager.instance.displayAbility(abilityInfo);

            inventoryMessageUpdate.text = pickUpMsg_New;
            gameManager.instance.inventoryMessageBox.SetActive(true);

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
            for (int i = 0; i < abilityBar.Count; i++)
            {
                if (stats == abilityBar[i])
                {                    
                    gameManager.instance.displayAbility(abilityInfo);
                    inventoryMessageUpdate.text = pickUpMsg_Own;
                    gameManager.instance.inventoryMessageBox.SetActive(true);
                    return;
                }
            }
            abilityBar.Add(stats);
            gameManager.instance.displayAbility(abilityInfo);

            inventoryMessageUpdate.text = pickUpMsg_Inventory;
            gameManager.instance.inventoryMessageBox.SetActive(true);

            if (inventorySlot1.GetComponent<Image>().sprite.name == "None2")
            {
                inventorySlot1.GetComponent<Image>().sprite = abilityImage;
            }
            else if (inventorySlot2.GetComponent<Image>().sprite.name == "None2")
            {
                inventorySlot2.GetComponent<Image>().sprite = abilityImage;
            }
            else if (inventorySlot3.GetComponent<Image>().sprite.name == "None2")
            {
                inventorySlot3.GetComponent<Image>().sprite = abilityImage;
            }
            else if (inventorySlot4.GetComponent<Image>().sprite.name == "None2")
            {
                inventorySlot4.GetComponent<Image>().sprite = abilityImage;
            }
            else if (inventorySlot5.GetComponent<Image>().sprite.name == "None2")
            {
                inventorySlot5.GetComponent<Image>().sprite = abilityImage;
            }
            else if (inventorySlot6.GetComponent<Image>().sprite.name == "None2")
            {
                inventorySlot6.GetComponent<Image>().sprite = abilityImage;
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
    public void hack()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, 100))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                hackTarget = hit.collider.gameObject;
                gameManager.instance.hackUI.SetActive(true);
                hackCounter = 0;
                StartCoroutine(beginHack(hackCounter));
            }
        }
    }
    public IEnumerator beginHack(int i)
    {
        if (i == 0)
        {
            gameManager.instance.hackInterface.GetComponent<Image>().sprite = hackAnimation[0];
            gameManager.instance.hackInterface.GetComponent<Image>().color = Color.white;
            gameManager.instance.hackError.SetActive(false);
            for (i = 1; i < hackAnimation.Count; i++)
            {
                yield return new WaitForSeconds(.33f);
                gameManager.instance.hackInterface.GetComponent<Image>().sprite = hackAnimation[i];
            }
            if (!cancelHack)
            {
                gameManager.instance.hackInterface.GetComponent<Image>().color = Color.green;
                StartCoroutine(hackTarget.GetComponent<enemyBossAI>().hacking(hackTarget));
                yield return new WaitForSeconds(2);
                gameManager.instance.hackUI.SetActive(false);
            }
        }
        else
        {
            cancelHack = true;
            gameManager.instance.hackError.SetActive(true);
            gameManager.instance.hackInterface.GetComponent<Image>().color = Color.red;
            yield return new WaitForSeconds(i);
            gameManager.instance.hackError.SetActive(false);
            gameManager.instance.hackUI.SetActive(false);
        }
    }
}
