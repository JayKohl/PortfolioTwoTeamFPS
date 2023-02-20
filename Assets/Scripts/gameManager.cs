using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] TextMeshProUGUI fuelCellsRemainingText;
    //Angel deleted this line of code
    //public GameObject textActivator;
    //temporary text was changed to info text
    public TextMeshProUGUI infoText;

    //Angel ADDED THIS CODE
    public TextMeshProUGUI npcChat;
    public GameObject playerChatBackground;
    public GameObject AbilityOne;
    public GameObject AbilityTwo;
    public GameObject AbilityThree;
    public GameObject AbilityFour;
    public AbilitiesColdown AbilityOneS;
    public AbilitiesColdown AbilityTwoS;
    public AbilitiesColdown AbilityThreeS;
    public AbilitiesColdown AbilityFourS;
    public bool ability;
    //Angel ADDED THIS CODE Above


    public GameObject muzzleFlash;
    public TextMeshProUGUI quickTexts;    
    public GameObject crosshair;
    public Sprite crosshairTexture;

    [Header("Goals")]
    public int fuelCellsRemaining;

    public bool isPaused;
    public bool bossDead;

    string goalsText;

    

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPosition = GameObject.FindGameObjectWithTag("Player Spawn Position");
        muzzleFlash = GameObject.FindGameObjectWithTag("MuzzleFlash");
        crosshair = GameObject.FindGameObjectWithTag("Crosshair");

        //Angel garcia
        ability = playerScript.abilityOneActive;
        AbilityOneS = AbilityOne.GetComponent<AbilitiesColdown>();
        AbilityTwoS = AbilityTwo.GetComponent<AbilitiesColdown>();
        AbilityThreeS = AbilityThree.GetComponent<AbilitiesColdown>();
        AbilityFourS = AbilityFour.GetComponent<AbilitiesColdown>();
        AbilityOneS.cooldownTime = 10f;
        AbilityTwoS.cooldownTime = 2f;
        AbilityFourS.cooldownTime = 12f;
    }

    // Update is called once per frame
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

        //Angel ADDED THIS CODE
        if(Input.GetKeyDown(KeyCode.Q) && AbilityOneS.wasSpellUsed() )
        {
            
            playerScript.StartCoroutine(playerScript.abilityCoolSpeed(4));
            AbilityOneS.wasSpellUsed();
            AbilityOneS.coolDownAbility();
        }
        if (Input.GetKeyDown(KeyCode.R) && AbilityTwoS.wasSpellUsed())
        {
            
            playerScript.StartCoroutine(playerScript.abilityCoolHeart(2));
            AbilityTwoS.wasSpellUsed();
            AbilityTwoS.coolDownAbility();
        }
        if (Input.GetKeyDown(KeyCode.E) && AbilityFourS.wasSpellUsed())
        {
            
            playerScript.StartCoroutine(playerScript.abilityCoolDash(12));
            AbilityFourS.wasSpellUsed();
            AbilityFourS.coolDownAbility();           
        }

    }
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
        fuelCellsRemaining = fuelCellsRemaining + amount;
        fuelCellsRemainingText.text = fuelCellsRemaining.ToString("F0");

        if (fuelCellsRemaining <= 0)
        {
            //send message to player to head to arena or something
            if(bossDead)
                StartCoroutine(end());
        }
    }
    IEnumerator end()
    {
        yield return new WaitForSeconds(2);
        pause();
        activeMenu = winMenu;
        activeMenu.SetActive(true);
    }
    public void playerDead()
    {
        pause();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }
    public IEnumerator checkPointDisplay()
    {
        quickTexts.text = "Checkpoint";
        yield return new WaitForSeconds(1);
        quickTexts.text = "";
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
    public void deleteTextNpc()
    {
        playerChatBackground.SetActive(false);
    }

}
