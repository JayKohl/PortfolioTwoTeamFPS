using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public int playerLevel;
    public int LevelMax = 10;
    public float currentXP;
    public float NeededXP;
    public int tokenAmount;
    public bool lvlScreenOn;
    [Header("Calculations")]
    [Range(0.01f, 0.6f)] public float VarX;
    [Range(1.0f, 3.5f)] public float VarY;

    //float lerpTimer;
    //float delayTimer;

    [SerializeField] public Image frontXPbar;
    // [SerializeField] public Image backXPbar;
    // [SerializeField] public Image tokenImage;

    // Start is called before the first frame update
    void Start()
    {
        frontXPbar = gameManager.instance.playerXPBar;
        frontXPbar.fillAmount = currentXP / NeededXP;
        //backXPbar.fillAmount = currentXP / NeededXP;
        //LevelUp();


    }

    // Update is called once per frame
    void Update()
    {
        UpdateXPBar();
        if (Input.GetKeyDown(KeyCode.Equals))
            GainExperiance(120);
        if (currentXP > NeededXP)
            LevelUp();
        if (Input.GetKeyDown("tab"))
        {
            if (lvlScreenOn)
            {
                //gameManager.instance.inventoryMessageBox.SetActive(false);
                lvlScreenOn = false;
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                gameManager.instance.lvlMenu.SetActive(false);
            }
            else
            {
                lvlScreenOn = true;
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                //inventoryMessageUpdate.text = defaultMsg;
                //gameManager.instance.inventoryMessageBox.SetActive(true);
                gameManager.instance.lvlMenu.SetActive(true);
            }
        }
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
        
        //float xpFraction = currentXP / NeededXP;
        //float Fxp = frontXPbar.fillAmount;
        //if (Fxp < xpFraction)
        //{
        //    delayTimer += Time.deltaTime;
        //    backXPbar.fillAmount = xpFraction;
        //    if (delayTimer > 3)
        //    {
        //        lerpTimer += Time.deltaTime;
        //        float percentComplete = lerpTimer / 4;
        //        frontXPbar.fillAmount = Mathf.Lerp(Fxp, backXPbar.fillAmount, percentComplete);
        //    }
        //}
    }

    public void GainExperiance(float gainedXP)
    {
        if (playerLevel < 10)
        {
            currentXP += gainedXP;
        }

        //lerpTimer = 0f;
        //delayTimer = 0f;
    }

    public void LevelUp()
    {
        playerLevel++;
        frontXPbar.fillAmount = 0f;
        //backXPbar.fillAmount = 0f;
        currentXP = Mathf.RoundToInt(currentXP - NeededXP);
        tokenAmount += playerLevel;
        gameManager.instance.playerScript.giveHP(gameManager.instance.playerScript.hpOriginal);
        NeededXP = CalculateXP();
    }

    private int CalculateXP()
    {

        int requiredxp = 0;
        return requiredxp = (int)Mathf.Floor(Mathf.Pow((playerLevel / VarX), VarY));
    }
}
