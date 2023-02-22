using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class terminalLvlTwo : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
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
            blowUp.SetActive(true);

            screenOne.SetActive(false);
            screenTwo.SetActive(false);
            screenThree.SetActive(false);

            laserOne.SetActive(false);
            laserTwo.SetActive(false);
            laserThree.SetActive(false);
            laserFour.SetActive(false);
            laserFive.SetActive(false);
            laserSix.SetActive(false);
            laserSeven.SetActive(false);
            laserEight.SetActive(false);
            GetComponent<Collider>().enabled = false;
            StartCoroutine(terminalDead());
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
        yield return new WaitForSeconds(2);
        brokenEffect.SetActive(true);
    }
}
