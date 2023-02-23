using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    // Start is called before the first frame update
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
}
