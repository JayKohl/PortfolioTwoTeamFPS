using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class terminalLvlOne : MonoBehaviour, IDamage
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

            blowUp.SetActive(true);
            screenOne.SetActive(false);
            screenTwo.SetActive(false);
            screenThree.SetActive(false);
            GetComponent<Collider>().enabled = false;

            StartCoroutine(turnOffLasers());
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
        brokenEffect.SetActive(true);
        yield return new WaitForSeconds(2);
    }
    IEnumerator turnOffLasers()
    {
        yield return new WaitForSeconds(1);
        laserOne.SetActive(false);


    }
}
