using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Breakable : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] AudioSource aud;
    [SerializeField] GameObject destroyEffect;
    [SerializeField] GameObject destroyed;

    [Range(1, 25)] [SerializeField] int HP;

    [Header("----- Sound -----")]
    [SerializeField] AudioClip[] audWallBreak;
    [Range(0, 1)] [SerializeField] float audWallBreakVol;
    [SerializeField] AudioClip[] audWallDie;
    [Range(0, 1)] [SerializeField] float audWallDieVol;

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        Vector3 Yoffset = new Vector3(0, 2, 0);

        if (HP <= 0)
        {
            aud.PlayOneShot(audWallDie[Random.Range(0, audWallDie.Length)], audWallDieVol);
            Instantiate(destroyEffect, transform.position, destroyEffect.transform.rotation);
            Destroy(gameObject);
            Instantiate(destroyed, (transform.position - Yoffset), destroyed.transform.rotation);
            
        }
        else
        {
            StartCoroutine(flashBreakDamage());
        }
    }
    IEnumerator flashBreakDamage()
    {
        aud.PlayOneShot(audWallBreak[Random.Range(0, audWallBreak.Length)], audWallBreakVol);
        model.material.color = Color.gray;
        yield return new WaitForSeconds(0.15f);
        model.material.color = Color.white;
    }

}
