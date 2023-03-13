using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonFunctions : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip resumeButton;
    [SerializeField] float soundVol = 0.2f;
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
        gameManager.instance.unPause();
        gameManager.instance.isPaused = !gameManager.instance.isPaused;
    }

    public void respawn()
    {
        aud = gameObject.GetComponent<AudioSource>();
        aud.PlayOneShot(resumeButton, soundVol);
        gameManager.instance.unPause();
        gameManager.instance.playerScript.playerRespawn();
        gameManager.instance.playerScript.playerDied = false;
    }
    
    public void restart()
    {
        aud = gameObject.GetComponent<AudioSource>();
        aud.PlayOneShot(resumeButton, soundVol);
        gameManager.instance.playerScript.poisoned = false;
        gameManager.instance.unPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void quit()
    {
        aud = gameObject.GetComponent<AudioSource>();
        aud.PlayOneShot(resumeButton, soundVol);
        Application.Quit();
    }
    public void menu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void start()
    {
        aud = gameObject.GetComponent<AudioSource>();
        aud.PlayOneShot(resumeButton, soundVol);
        SceneManager.LoadScene("CrawlScene");
    }
    public void startGame()
    {
        SceneManager.LoadScene("LvlOneArena");
    }
    public void startLevel2()
    {
        SceneManager.LoadScene("LvlTwoTheArena");
    }
    public void startLevel3()
    {
        aud = gameObject.GetComponent<AudioSource>();
        aud.PlayOneShot(resumeButton, soundVol);
        SceneManager.LoadScene("LvlThreeTheWorld");
    }
    public void credits()
    {
        aud = gameObject.GetComponent<AudioSource>();
        aud.PlayOneShot(resumeButton, soundVol);
        SceneManager.LoadScene("End Credits");
    }

    public void Options()
    {
        gameManager.instance.activeMenu.SetActive(false);
        gameManager.instance.activeMenu = gameManager.instance.optionMenu;
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
