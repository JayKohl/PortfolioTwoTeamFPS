using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class temporaryTexts : MonoBehaviour
{
    bool playerIn;
    
    Color thisColor = Color.blue;
    Color originalColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        thisColor.a = 0.20f;
        originalColor.a = 0.20f;
    }

    // Update is called once per frame
    void Update()
    {

        if (playerIn)
        {
            gameManager.instance.updateGameGoal(-1);
            playerIn = false;
            StartCoroutine(flashOnPick());
            Destroy(gameObject, 0.15f);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIn = true;
        }
    }

    IEnumerator flashOnPick()
    {
        gameManager.instance.playerDamageFlashScreen.GetComponent<Image>().color = thisColor;
        gameManager.instance.playerDamageFlashScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDamageFlashScreen.SetActive(false);
        gameManager.instance.playerDamageFlashScreen.GetComponent<Image>().color = originalColor;
    }
 //Code ends
}
