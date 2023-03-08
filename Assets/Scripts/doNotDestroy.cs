using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class doNotDestroy : MonoBehaviour
{
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "LvlTwoTheArena")
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
