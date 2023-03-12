using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lvl3bossTriggerEvent : MonoBehaviour
{
    [SerializeField] GameObject laserField;
    bool playerIn;
    bool isGoalReset;

    private void Start()
    {
        laserField.SetActive(false);
        isGoalReset = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!playerIn)
        {
            if (other.CompareTag("Player"))
            {
                if (isGoalReset == false)
                {
                    gameManager.instance.enemiesRemaining = 0;
                    isGoalReset = true;
                }
                playerIn = true;
                laserField.SetActive(true);
                gameManager.instance.infoText.text = "Kill the alien horde's energy source.";
                gameManager.instance.infoTextBackground.SetActive(true);
            }
        }
    }
}
