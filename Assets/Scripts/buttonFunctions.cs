using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonFunctions : MonoBehaviour
{
    [SerializeField] public AudioSource aud;
    [SerializeField] AudioClip resumeButton;
    [SerializeField] [Range(0, 1)] float buttonVol;
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
        gameManager.instance.unPause();
        gameManager.instance.playerScript.playerRespawn();
    }
    
    public void restart()
    {
        gameManager.instance.unPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quit()
    {
        Application.Quit();
    }
    public void menu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void start()
    {
        SceneManager.LoadScene("CrawlScene");
    }
    public void startGame()
    {
        SceneManager.LoadScene("LvlOneArena");
    }
    public void startLevel2()
    {
        SceneManager.LoadScene("LvlTwoTheArena");
        resume();
    }
    public void startLevel3()
    {
        aud.PlayOneShot(resumeButton,buttonVol);
        SceneManager.LoadScene("LvlThreeTheWorld");
        resume();
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
