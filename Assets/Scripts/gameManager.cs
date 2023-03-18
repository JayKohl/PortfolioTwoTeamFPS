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
    [SerializeField] public GameObject playerCamera;
    [SerializeField] public GameObject cam2;
    [Header("UI")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject optionMenu;
    public GameObject playerDamageFlashScreen;
    [SerializeField] public Image playerHPBar;
    [SerializeField] public Image playerXPBar;
    [SerializeField] public GameObject fuelCellsRemainingObject;
    [SerializeField] TextMeshProUGUI fuelCellsRemainingText;
    [SerializeField] public GameObject enemiesRemainingObject;
    [SerializeField] public TextMeshProUGUI enemiesRemainingText;

    [SerializeField] GameObject npc;
    [SerializeField] GameObject minimap;

    public GameObject infoTextBackground;
    public TextMeshProUGUI infoText;

    public TextMeshProUGUI npcChat;
    public TextMeshProUGUI npcChat2;
    public GameObject playerChatBackground;
    [SerializeField] public GameObject abilityHub;
    [SerializeField] public GameObject abilityDisplay;
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

    public GameObject inventory;
    public GameObject inventoryMessageBox;
    public TextMeshProUGUI inventoryMessages;
    public GameObject inventorySlot1;
    public GameObject inventorySlot2;
    public GameObject inventorySlot3;
    public GameObject inventorySlot4;
    public GameObject inventorySlot5;
    public GameObject inventorySlot6;

    public GameObject lvlMenu;
    public GameObject firstTimeText;
    public LevelSystem lvlscript;
    public LVLButtons lvlbuttons;
    public ModifyText modTextScript;

    public GameObject muzzleFlash;
    public GameObject quickTexts;
    public GameObject crosshair;
    public Sprite crosshairTexture;
    public bool shieldOn;
    [SerializeField] public GameObject shieldUI;
    [SerializeField] public GameObject shieldHP;
    public int shieldHPNum;
    [SerializeField] public GameObject invisUI;
    [SerializeField] public GameObject dashUI;
    [SerializeField] public GameObject hackUI;
    [SerializeField] public GameObject hackInterface;
    [SerializeField] public GameObject hackError;

    [SerializeField] public AudioSource aud;
    [SerializeField] public AudioClip error;
    [SerializeField] public AudioClip notify;

    [Header("Goals")]
    public int fuelCellsRemaining;
    public int enemiesRemaining;

    public bool isPaused;
    public bool bossDead;
    public bool boss2Dead = false;    
    public bool flightDeck = false;
    public bool boss3Dead = false;
    public bool inCutscene;

    string goalsText;
    [SerializeField] public GameObject endGameTrigger;
    [SerializeField] public float soundVol = 0.2f;
    [SerializeField] public float musicVol = 0.2f;


    void Awake()
    {
        
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        lvlscript = player.GetComponent<LevelSystem>();
        lvlbuttons = player.GetComponent<LVLButtons>();
        modTextScript = pauseMenu.GetComponentInChildren<ModifyText>();
        playerSpawnPosition = GameObject.FindGameObjectWithTag("Player Spawn Position");
        muzzleFlash = GameObject.FindGameObjectWithTag("MuzzleFlash");
        npcChat2 = GameObject.FindGameObjectWithTag("NPCChat").GetComponentInChildren<TextMeshProUGUI>();

        //cam2 = GameObject.FindGameObjectWithTag("Camera2");

        ability = playerScript.abilityOneActive;
        AbilityOneS = AbilityOne.GetComponent<AbilitiesColdown>();
        AbilityTwoS = AbilityTwo.GetComponent<AbilitiesColdown>();
        AbilityThreeS = AbilityThree.GetComponent<AbilitiesColdown>();
        AbilityFourS = AbilityFour.GetComponent<AbilitiesColdown>();
        AbilityOneS.cooldownTime = 10f;
        AbilityTwoS.cooldownTime = 10f;
        AbilityFourS.cooldownTime = 12f;
        minimap = GameObject.FindGameObjectWithTag("Minimap");

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

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (inventory.activeSelf || lvlMenu.activeSelf)
            {
                aud.PlayOneShot(gameManager.instance.abilityHub.GetComponent<activateAbility>().inventoryOpen);
            }
            lvlMenu.SetActive(false);
            inventory.SetActive(false);
            inventoryMessageBox.SetActive(false);
            abilityDisplay.SetActive(false);
            crosshair.SetActive(true);
            unPause();
            isPaused = false;
        }
    }

    public void pause()
    {
        gameManager.instance.playerScript.canShoot = false;
        abilityHub.GetComponent<activateAbility>().inventoryScreenOn = false;
        inventory.SetActive(false);
        inventoryMessageBox.SetActive(false);        
        lvlscript.lvlScreenOn = false;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        modTextScript.HintSelect();
        lvlMenu.SetActive(false);
    }
    public void unPause()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
        }
        activeMenu = null;
        gameManager.instance.playerScript.canShoot = true;
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
        SceneManager.LoadScene("Part3Scene");
    }
    public void updateGameGoalLvl3(int amount)
    {
        enemiesRemaining += amount;
        if(boss3Dead)
        {
            minimap.SetActive(false);
            pause();
            activeMenu = winMenu;
            activeMenu.SetActive(true);
            inCutscene = true;
            StartCoroutine(ending());
        }
    }
    IEnumerator ending()
    {
        Time.timeScale = 1;
        gameManager.instance.player.GetComponent<Collider>().enabled = false;
        Cursor.visible = false;
        yield return new WaitForSeconds(3);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        gameManager.instance.playerScript.canShoot = false;
        SceneManager.LoadScene("End Credits");
    }
    public void playerDead()
    {
        if (inCutscene == false)
        {
            playerScript.playerDied = true;
            minimap.SetActive(false);
            hackUI.SetActive(false);
            pause();
            activeMenu = loseMenu;
            activeMenu.SetActive(true);
        }
    }
    public IEnumerator notificationDisplay(notifications textureDisp)
    {
        crosshair.SetActive(false);
        quickTexts.GetComponent<Image>().sprite = textureDisp.textureToDisplay;
        quickTexts.SetActive(true);
        yield return new WaitForSeconds(3f);
        quickTexts.SetActive(false);
        crosshair.SetActive(true);
    }
    public void displayAbility(Sprite abilityTexture)
    {
        crosshair.SetActive(false);
        abilityDisplay.GetComponent<Image>().sprite = abilityTexture;
        abilityDisplay.SetActive(true);
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
    public void displayNpcCinematic(string textToDisplay)
    {
        npcChat2.SetText(textToDisplay);
        playerChatBackground.SetActive(false);
    }

    public IEnumerator deleteText(float banishTime)
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
