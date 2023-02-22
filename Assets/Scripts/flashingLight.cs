using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashingLight : MonoBehaviour
{
    [SerializeField] GameObject redLights;
    // Update is called once per frame
    void Update()
    {
        redLights.SetActive(false);
        StartCoroutine(flash());
    }
    IEnumerator flash()
    {
        yield return new WaitForSeconds(3);
        redLights.SetActive(true);
    }
}
