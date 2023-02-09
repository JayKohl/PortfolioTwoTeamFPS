using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class crawlSceneButton : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}