using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashingLight : MonoBehaviour
{
    [SerializeField] GameObject redLights;
    [SerializeField] GameObject redBulb;
    bool isFlashing;
    bool isFirstRound;
    bool isSecondRound;

    private void Start()
    {
        isFirstRound = true;
        isSecondRound = false;
        isFlashing = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (isFlashing == false)
        {
            isFlashing = true;
            if (isFirstRound == true)
            {
                redLights.SetActive(false);
                redBulb.SetActive(false);
                isSecondRound = true;
                isFirstRound = false;
            }
            //StartCoroutine(waitTurnOff());
            StartCoroutine(flash());
        }
    }
    IEnumerator flash()
    {
        yield return new WaitForSeconds(3);
        if (isSecondRound == true)
        {
            redLights.SetActive(true);
            redBulb.SetActive(true);
            isSecondRound = false;
            isFirstRound = true;
        }
        isFlashing = false;
    }
    //IEnumerator waitTurnOff()
    //{
    //    yield return new WaitForSeconds(1);
    //}
}
