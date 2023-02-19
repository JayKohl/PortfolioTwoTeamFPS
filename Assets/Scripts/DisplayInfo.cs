using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayInfo : MonoBehaviour
{
    string text = "Display this info please... Yes!";
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.displayText(text);

        }
    }   
}
