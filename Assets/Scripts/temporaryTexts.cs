using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class temporaryTexts : MonoBehaviour
{
    bool playerIn;
    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {

        if (playerIn)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIn = true;

        }
    }
}
