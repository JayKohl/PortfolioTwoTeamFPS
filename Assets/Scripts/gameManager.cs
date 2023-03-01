using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("Player")]
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPosition;

    [Header("UI")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject playerDamageFlashScreen;
    public Image playerHPBar;
    [SerializeField] public GameObject fuelCellsRemainingObject;
    [SerializeField] TextMeshProUGUI fuelCellsRemainingText;
    [SerializeField] public GameObject enemiesRemainingObject;
    [SerializeField] TextMeshProUGUI enemiesRemainingText;

    public GameObject infoTextBackground;
    public TextMeshProUGUI infoText;

    public TextMeshProUGUI npcChat;
    public GameObject playerChatBackground;
    public GameObject AbilitiesBackground;
    public GameObject AbilityOne;
    public GameObject AbilityTwo;
    public GameObject AbilityThree;
    public GameObject AbilityFour;
    public AbilitiesColdown AbilityOneS;
    public AbilitiesColdown AbilityTwoS;
    public AbilitiesColdown AbilityThreeS;
    public AbilitiesColdown AbilityFourS;
    public bool ability;

    public GameObject muzzleFlash;
    public GameObject quickTexts;
    public GameObject crosshair;
    public Sprite crosshairTexture;
    public bool shieldOn;
    [SerializeField] public GameObject shieldUI;
    [SerializeField] public GameObject invisUI;
    [SerializeField] public GameObject dashUI;

    [SerializeField] AudioSource aud;
    [SerializeField] public AudioClip invisOnAud;
    [Range(0, 1)] [SerializeField] public float invisOnVol;
    [SerializeField] public AudioClip invisOffAud;
    [Range(0, 1)] [SerializeField] public float invisOffVol;
    [SerializeField] public AudioClip dashAud;
    [Range(0, 1)] [SerializeField] public float dashVol;

    [Header("Goals")]
    public int fuelCellsRemaining;
    public int enemiesRemaining;

    public bool isPaused;
    public bool bossDead;
    public bool boss2Dead = false;
    public bool flightDeck = false;

    string goalsText;
    [SerializeField] public GameObject endGameTrigger;


    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPosition = GameObject.FindGameObjectWithTag("Player Spawn Position");
        muzzleFlash = GameObject.FindGameObjectWithTag("MuzzleFlash");

        ability = playerScript.abilityOneActive;
        AbilityOneS = AbilityOne.GetComponent<AbilitiesColdown>();
        AbilityTwoS = AbilityTwo.GetComponent<AbilitiesColdown>();
        AbilityThreeS = AbilityThree.GetComponent<AbilitiesColdown>();
        AbilityFourS = AbilityFour.GetComponent<AbilitiesColdown>();
        AbilityOneS.cooldownTime = 10f;
        AbilityTwoS.cooldownTime = 10f;
        AbilityFourS.cooldownTime = 12f;
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            pauseMenu.SetActive(isPaused);

            if (isPaused)
                pause();
            else
                unPause();
        }

        /*if (Input.GetKeyDown(KeyCode.Q) && AbilityOneS.wasSpellUsed())
        {
            if(AbilityOne.activeSelf)
            {
                gameManager.instance.playerScript.throwGrenade();
                AbilityOneS.wasSpellUsed();
                AbilityOneS.coolDownAbility();
            }            
        }*/
        if (Input.GetKeyDown(KeyCode.R) && AbilityTwoS.wasSpellUsed())
        {
            if (AbilityTwo.activeSelf)
            {
                aud.PlayOneShot(invisOffAud, invisOffVol);
                StartCoroutine(playerScript.abilityCoolShield(playerScript.shieldOnPlayer.GetComponent<shield>().shieldTimer));
            }
        }
        if (Input.GetKeyDown(KeyCode.F) && AbilityThreeS.wasSpellUsed())
        {
            if (AbilityThree.activeSelf)
            {
                aud.PlayOneShot(invisOnAud, invisOnVol);
                gameManager.instance.playerScript.invisibility();
                AbilityThreeS.wasSpellUsed();
                AbilityThreeS.coolDownAbility();
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && AbilityFourS.wasSpellUsed())
        {
            if (AbilityFour.activeSelf)
            {                
                aud.PlayOneShot(dashAud, dashVol);
                playerScript.StartCoroutine(playerScript.abilityCoolDash(12));
                AbilityFourS.wasSpellUsed();
                AbilityFourS.coolDownAbility();
            }
        }
    }
    //public void shieldCoolDown()
    //{
    //    AbilityTwoS.wasSpellUsed();
    //    AbilityTwoS.coolDownAbility();
    //}

    public void pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void unPause()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }
    public void updateGameGoal(int amount)
    {
        fuelCellsRemaining += amount;
        fuelCellsRemainingText.text = fuelCellsRemaining.ToString("F0");

        if (fuelCellsRemaining <= 0)
        {
            //send message to player to head to arena or something
            if (bossDead)
                StartCoroutine(endLevel1());
        }
    }
    IEnumerator endLevel1()
    {
        yield return new WaitForSeconds(2);
        pause();
        SceneManager.LoadScene("Part2Scene");
    }
    public void updateGameGoalLvl2(int amount)
    {
        enemiesRemaining += amount;
        enemiesRemainingText.text = enemiesRemaining.ToString("F0");
        if(enemiesRemaining <= 0)
        {
            enemiesRemainingObject.SetActive(false);
        }

        if(boss2Dead && flightDeck && enemiesRemaining <= 0)
        {
            gameManager.instance.infoText.text = "<s>Get to the flight deck</s>" + "\n<s>Kill the radiated bug</s>"+"\nEscape!";
            gameManager.instance.infoTextBackground.SetActive(true);            
        }        
    }
    public IEnumerator endLevel2()
    {
        yield return new WaitForSeconds(2);
        pause();
        //change winMenu text for level 2
        activeMenu = winMenu;
        activeMenu.SetActive(true);
    }
    public void playerDead()
    {
        pause();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }
    public IEnumerator checkPointDisplay(notifications textureDisp)
    {        
        quickTexts.GetComponent<Image>().sprite = textureDisp.textureToDisplay;
        quickTexts.SetActive(true);
        yield return new WaitForSeconds(1f);
        quickTexts.SetActive(false);
    }
    public void displayText(string textToDisplay)
    {
        infoText.SetText(textToDisplay);
        goalsText = textToDisplay;
    }

    public void goalsDisplayText()
    {
        infoText.SetText(goalsText);
    }

    public void displayNpcText(string textToDisplay)
    {
        npcChat.SetText(textToDisplay);
        playerChatBackground.SetActive(true);
    }

    IEnumerator deleteText(float banishTime)
    {
        yield return new WaitForSeconds(banishTime);
        gameManager.instance.infoText.SetText(" ");
    }
    public IEnumerator deleteTextNpc(float banishTime)
    {
        yield return new WaitForSeconds(banishTime);
        playerChatBackground.SetActive(false);
    }

}
