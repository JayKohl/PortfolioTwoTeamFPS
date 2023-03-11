using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class terminalLvlTwo : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] GameObject alarmTrigger;
    [Header("----- Terminal Stats -----")]
    [SerializeField] int hitPoints;
    [Header("----- Terminal Monitor -----")]
    [SerializeField] GameObject screenOne;
    [SerializeField] GameObject screenTwo;
    [SerializeField] GameObject screenThree;
    [Header("----- Hallway Lasers -----")]
    [SerializeField] GameObject laserOne;
    [SerializeField] GameObject laserTwo;
    [SerializeField] GameObject laserThree;
    [SerializeField] GameObject laserFour;
    [SerializeField] GameObject laserFive;
    [SerializeField] GameObject laserSix;
    [SerializeField] GameObject laserSeven;
    [SerializeField] GameObject laserEight;
    [Header("----- Explosions -----")]
    [SerializeField] GameObject damageSparks;
    [SerializeField] GameObject blowUp;
    [SerializeField] GameObject brokenEffect;


    public virtual void takeDamage(int dmg)
    {
        hitPoints -= dmg;
        if (hitPoints <= 0)
        {
            terminalDead();

            gameManager.instance.playerScript.minimap.SetActive(false);
            gameManager.instance.playerHPBar.transform.parent.gameObject.SetActive(false);
            gameManager.instance.enemiesRemainingObject.SetActive(false);
            gameManager.instance.enemiesRemainingText.enabled = false;
            gameManager.instance.crosshair.SetActive(false);            
            StartCoroutine(gameManager.instance.cam2.GetComponentInChildren<secondCamera>().openDoorThree());
            gameManager.instance.cam2.SetActive(true);
            gameManager.instance.playerCamera.SetActive(false);            


            blowUp.SetActive(true);
            screenOne.SetActive(false);
            screenTwo.SetActive(false);
            screenThree.SetActive(false);          
            GetComponent<Collider>().enabled = false;
            
            StartCoroutine(turnOffLasers());
            StartCoroutine(gameManager.instance.cam2.GetComponentInChildren<secondCamera>().doorThreeStop());



            //laserOne.SetActive(false);
            //laserTwo.SetActive(false);
            //laserThree.SetActive(false);
            //laserFour.SetActive(false);
            //laserFive.SetActive(false);
            //laserSix.SetActive(false);
            //laserSeven.SetActive(false);
            //laserEight.SetActive(false);
        }
        else
        {
            StartCoroutine(flashDamage());
            damageSparks.SetActive(true);
        }
    }
    protected IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        model.material.color = Color.white;
    }
    IEnumerator terminalDead()
    {
        alarmTrigger.GetComponent<alarmOnTrigger>().alarmOff();
        brokenEffect.SetActive(true);
        yield return new WaitForSeconds(2);
    }
    IEnumerator turnOffLasers()
    {
        yield return new WaitForSeconds(1);
        laserOne.SetActive(false);
        laserTwo.SetActive(false);
        laserThree.SetActive(false);
        laserFour.SetActive(false);
        laserFive.SetActive(false);
        laserSix.SetActive(false);
        laserSeven.SetActive(false);
        laserEight.SetActive(false);
       

    }
}
