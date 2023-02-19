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
    //Angel added this line
    public TextMeshProUGUI npcChat;
    public GameObject playerChatBackground;


    public GameObject muzzleFlash;

    [Header("Goals")]
    public int fuelCellsRemaining;

    public bool isPaused;
    public bool bossDead;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPosition = GameObject.FindGameObjectWithTag("Player Spawn Position");
        muzzleFlash = GameObject.FindGameObjectWithTag("MuzzleFlash");
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
        //checkPointPopUp.SetActive(true);
        yield return new WaitForSeconds(2);
        //checkPointPopUp.SetActive(false);
    }
    //Angel added this void methods
    public void displayText(string texToDisplay, float banishTime = .5f)
    {
        infoText.SetText(texToDisplay);
        StartCoroutine(deleteText(banishTime));
    }

    public void displayNpcText(string texToDisplay, float banishTime = .5f)
    {
        npcChat.SetText(texToDisplay);
        playerChatBackground.SetActive(true);
        StartCoroutine(deleteTextNpc(banishTime));
    }

    IEnumerator deleteText(float banishTime)
    {
        yield return new WaitForSeconds(banishTime);
        gameManager.instance.infoText.SetText(" ");
    }
    IEnumerator deleteTextNpc(float banishTime)
    {
        yield return new WaitForSeconds(banishTime);
        playerChatBackground.SetActive(false);
    }
}
