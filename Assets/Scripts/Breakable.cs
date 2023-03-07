using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Breakable : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] AudioSource aud;
    [SerializeField] GameObject destroyEffect;

    [Range(1, 25)] [SerializeField] int HP;
    
    [SerializeField] AudioClip[] audWallBreak;
    [Range(0, 1)] [SerializeField] float audWallBreakVol;

    public void takeDamage(int dmg)
    {
        HP -= dmg;

        if (HP <= 0)
        {
            //aud.PlayOneShot(audWallBreak[Random.Range(0, audWallBreak.Length)], audWallBreakVol);
            //Instantiate(destroyEffect, transform.position, destroyEffect.transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(flashBreakDamage());
        }
    }
    IEnumerator flashBreakDamage()
    {
        model.material.color = Color.gray;
        yield return new WaitForSeconds(0.15f);
        model.material.color = Color.white;
    }

}
