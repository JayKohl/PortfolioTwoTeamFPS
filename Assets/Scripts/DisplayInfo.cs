using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI thisText;
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.temporalText.text = thisText.text;
            gameManager.instance.textActivator.SetActive(true);
 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(spawnText());   
        }
    }

    IEnumerator spawnText()
    {
        yield return new WaitForSeconds(4);
        gameManager.instance.textActivator.SetActive(false);
        Destroy(gameObject);
    }
}
