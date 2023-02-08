using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TemporalTexts : MonoBehaviour
{

    
    [SerializeField] GameObject textHolder;
   

    bool playerIn;
    // Start is called before the first frame update
    void Start()
    {
        textHolder.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        textHolder = GameObject.FindGameObjectWithTag("TextHolder");
       
        if(playerIn)
        {
            StartCoroutine(ShowText());
            Destroy(gameObject, 10);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIn = true;

        }
    }

    IEnumerator ShowText()
    {
        playerIn = true;
        textHolder.SetActive(true);
        yield return new WaitForSeconds(5f);
        playerIn = false;
        textHolder.SetActive(false);
    }
}
