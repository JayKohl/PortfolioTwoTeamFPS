using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSystem : MonoBehaviour
{
    public bool firsttime = true;
    public int playerLevel;
    public int LevelMax = 10;
    public float currentXP;
    public float NeededXP;
    public int tokenAmount;
    public bool lvlScreenOn;
    public GameObject lvlUpText;
    public float XPMod = 1;
    public GameObject lvlMenuInformation;
    public bool infoOn = false;
    //public Vector3 startPosition;
    //[SerializeField] public Vector3 endPosition;
    //[SerializeField] public float Timelength;

    AudioSource aud;
    [SerializeField] public TextMeshProUGUI tokentext;
    [Header("Calculations")]
    [Range(0.01f, 0.6f)] public float VarX;
    [Range(1.0f, 3.5f)] public float VarY;


    [SerializeField] public Image frontXPbar;
    

    void Start()
    {
        aud = gameManager.instance.aud;
        frontXPbar = gameManager.instance.playerXPBar;
        frontXPbar.fillAmount = currentXP / NeededXP;
        //startPosition = lvlUpText.transform.position;
        
    }

    
    void Update()
    {
        UpdateXPBar();
        tokentext.text = tokenAmount.ToString("F0");
        //if (Input.GetKeyDown(KeyCode.Equals))
        //    GainExperiance(120);
        if (currentXP > NeededXP)
            LevelUp();
        if (Input.GetKeyDown("tab") && !gameManager.instance.isPaused)
        {
            if (lvlScreenOn)
            {
                aud.PlayOneShot(gameManager.instance.playerScript.lvlUp);
                lvlScreenOn = false;
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                gameManager.instance.lvlMenu.SetActive(false);
                
                
            }
            else
            {
                aud.PlayOneShot(gameManager.instance.playerScript.lvlUp);
                lvlScreenOn = true;
                if (firsttime)
                {                    
                    StartCoroutine(firstTimeInfo());
                    firsttime = false;
                }
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                gameManager.instance.lvlMenu.SetActive(true);

               
            }

            
        }
    }

    public int GetTokens()
    {
        return tokenAmount;
    }
    public void DecrementTokens(int used)
    {
        tokenAmount -= used;
    }

    public void UpdateXPBar()
    {
        if (playerLevel < LevelMax)
        {
            gameManager.instance.playerXPBar.fillAmount = currentXP / NeededXP;
        }
        else
        {
            gameManager.instance.playerXPBar.fillAmount = 1;
        }
    }

    public void GainExperiance(float gainedXP)
    {
        if (playerLevel < 10)
        {
            currentXP += gainedXP * XPMod;

            if (!infoOn && SceneManager.GetActiveScene().name == "LvlOneArena")
            {
                StartCoroutine(LVLInfotext());
                tokenAmount++;
            }
        }
    }

    public void LevelUp()
    {
        playerLevel++;
        frontXPbar.fillAmount = 0f;
        currentXP = Mathf.RoundToInt(currentXP - NeededXP);
        tokenAmount += playerLevel/2;
        gameManager.instance.playerScript.aud.PlayOneShot(gameManager.instance.playerScript.lvlUp, gameManager.instance.soundVol);
        StartCoroutine(LevelText());
        NeededXP = CalculateXP();
    }

    private int CalculateXP()
    {
        int requiredxp = 0;
        return requiredxp = (int)Mathf.Floor(Mathf.Pow((playerLevel / VarX), VarY));
    }

    IEnumerator firstTimeInfo()
    {
        gameManager.instance.firstTimeText.SetActive(true);

        yield return new WaitForSecondsRealtime(2.5f);

        gameManager.instance.firstTimeText.SetActive(false);
    }

    IEnumerator LevelText()
    {
        lvlUpText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        lvlUpText.SetActive(false);
    }

    IEnumerator LVLInfotext()
    {
        lvlMenuInformation.SetActive(true);
        gameManager.instance.pause();
        infoOn = true;
        yield return new WaitForSecondsRealtime(3);
        lvlMenuInformation.SetActive(false);
        gameManager.instance.unPause();

    }

    //IEnumerator TextMoveInFrame()
    //{
    //    float timer = 0;
        
    //    while (timer < Timelength)
    //    {
    //        transform.position = Vector3.Lerp(startPosition, endPosition, timer / Timelength);
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }
    //    lvlUpText.transform.position = endPosition;
    //}
}
