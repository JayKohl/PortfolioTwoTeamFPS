using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scrollingText : MonoBehaviour
{
    [Header("-----Settings-----")]
    [TextArea][SerializeField] private string[] itemInfo;
    [SerializeField] private float textSpeed = 0.01f;

    [Header("-----UI-----")]
    [SerializeField] private TextMeshProUGUI itemInfoText;

    private void Start()
    {
        StartCoroutine(scrollText());
    }
    IEnumerator scrollText()
    {
        for(int i = 0; i < itemInfo.Length; i++)
        {
            itemInfoText.text = itemInfo[i].ToString();
            yield return new WaitForSeconds(textSpeed);
        }        
    }    
}
