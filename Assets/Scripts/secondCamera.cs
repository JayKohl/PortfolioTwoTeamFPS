using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class secondCamera : MonoBehaviour
{
    [SerializeField] public Transform locDoorOne;
    [SerializeField] public Transform locDoorTwo;
    [SerializeField] public Transform locDoorThree;

    [SerializeField] public GameObject doorOne;
    [SerializeField] public GameObject doorTwo;
    public Camera cam2;

    public void openDoorOne()
    {
        transform.position = locDoorOne.transform.position;
        transform.eulerAngles = locDoorOne.transform.eulerAngles;
        //cam2.fieldOfView = Mathf.Lerp(cam2.fieldOfView, 50, 40);

        //gameManager.instance.cam2.fieldOfView = Mathf.Lerp(gameManager.instance.cam2.fieldOfView, 80, 4 * Time.unscaledDeltaTime);
    }

    public void openDoorTwo()
    {
        transform.position = locDoorTwo.transform.position;
        transform.eulerAngles = locDoorTwo.transform.eulerAngles;
        //cam2.fieldOfView = Mathf.Lerp(cam2.fieldOfView, 50, 40 * Time.deltaTime);
        //gameManager.instance.cam2.fieldOfView = Mathf.Lerp(gameManager.instance.cam2.fieldOfView, 80, 4 * Time.unscaledDeltaTime);
    }
    public IEnumerator openDoorThree()
    {
        transform.position = locDoorThree.transform.position;
        transform.eulerAngles = locDoorThree.transform.eulerAngles;
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 0;
        //cam2.fieldOfView = Mathf.Lerp(cam2.fieldOfView, 50, 40 * Time.deltaTime);
        //gameManager.instance.cam2.fieldOfView = Mathf.Lerp(gameManager.instance.cam2.fieldOfView, 80, 4 * Time.unscaledDeltaTime);
    }
  
    
    public IEnumerator doorThreeStop()
    {        
        yield return new WaitForSecondsRealtime(3);        
        gameManager.instance.playerScript.controller.enabled = true;
        gameManager.instance.playerCamera.SetActive(true);
        gameManager.instance.playerScript.canShoot = true;
        gameManager.instance.cam2.SetActive(false);
        gameManager.instance.playerScript.minimap.SetActive(true);
        gameManager.instance.playerHPBar.transform.parent.gameObject.SetActive(true);
        gameManager.instance.enemiesRemainingObject.SetActive(true);
        gameManager.instance.enemiesRemainingText.enabled = true;
        gameManager.instance.crosshair.SetActive(true);
        gameManager.instance.unPause();
        yield return new WaitForSecondsRealtime(2);
        gameManager.instance.inCutscene = false;

    }
}