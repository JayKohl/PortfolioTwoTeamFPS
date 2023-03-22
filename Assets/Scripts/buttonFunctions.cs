using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonFunctions : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip resumeButton;
    //[SerializeField] float soundVol = 0.2f;
    //public bool click;
    //public friendlyAI friendNPC;
    //[SerializeField] Button theButton;
    //Start is called before the first frame update

    //private void Start()
    //{
    //    friendNPC = GetComponent<friendlyAI>();
    //}
    
    public void resume()
    {
        //aud.PlayOneShot(resumeButton);
        gameManager.instance.unPause();
        gameManager.instance.isPaused = false;
    }

    public void respawn()
    {
        aud.PlayOneShot(resumeButton);
        gameManager.instance.unPause();
        gameManager.instance.playerScript.playerRespawn();
        gameManager.instance.playerScript.playerDied = false;
        
    }
    
    public void restart()
    {
        aud.PlayOneShot(resumeButton);
        gameManager.instance.playerScript.poisoned = false;
        gameManager.instance.unPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void quit()
    {
        aud.PlayOneShot(resumeButton);
        Application.Quit();
    }
    public void menu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void start()
    {
        aud.PlayOneShot(resumeButton);
        SceneManager.LoadScene("CrawlScene");
    }
    public void startGame()
    {
        Time.timeScale = 1;
        aud.PlayOneShot(resumeButton);
        SceneManager.LoadScene("LvlOneArena");
    }
    public void startLevel2()
    {
        Time.timeScale = 1;
        aud.PlayOneShot(resumeButton);
        SceneManager.LoadScene("LvlTwoTheArena");
    }
    public void startLevel3()
    {
        Time.timeScale = 1;
        aud.PlayOneShot(resumeButton);
        SceneManager.LoadScene("LvlThreeTheWorld");
    }
    public void credits()
    {
        Time.timeScale = 1;
        aud.PlayOneShot(resumeButton);
        SceneManager.LoadScene("End Credits");
    }

    public void Options()
    {
        aud.PlayOneShot(resumeButton);
        gameManager.instance.activeMenu.SetActive(false);
        gameManager.instance.activeMenu = gameManager.instance.optionMenu;
        gameManager.instance.activeMenu.SetActive(true);

    }

    public void back()
    {
        gameManager.instance.activeMenu.SetActive(false);
        gameManager.instance.activeMenu = gameManager.instance.pauseMenu;
        gameManager.instance.activeMenu.SetActive(true);
    }

    //public void closeQuestOne()
    //{
    //    StartCoroutine(gameManager.instance.deleteTextNpc(0.1f));
    //    Destroy(friendNPC.doorToBoss);
    //    gameManager.instance.playerCamera.SetActive(true);
    //    friendNPC.cam2.SetActive(false);
    //    Cursor.visible = false;
    //    Cursor.lockState = CursorLockMode.Locked;
    //    friendNPC.friend.transform.position = friendNPC.orgPos.position;
    //    friendNPC.friend.transform.localRotation = friendNPC.orgPos.localRotation;
    //    gameManager.instance.unPause();
    //    click = true;

    //}
}
