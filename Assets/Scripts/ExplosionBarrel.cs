using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBarrel : MonoBehaviour, IDamage
{
    [SerializeField] AudioSource aud;    
    [SerializeField] GameObject BarrelExplosion;
    [Range(1, 5)] [SerializeField] int HP;

    [Header("----- Sound -----")]
    [SerializeField] AudioClip[] audBarrelExplode;
    [Range(0, 1)] [SerializeField] float audBarrelExplodeVol;


    public void takeDamage(int dmg)
    {
        HP -= dmg;

        if (HP <= 0)
        {
            //aud.PlayOneShot(audBarrelExplode[Random.Range(0, audBarrelExplode.Length)], audBarrelExplodeVol);
            Destroy(gameObject);
            Instantiate(BarrelExplosion, transform.position, BarrelExplosion.transform.rotation);
        }
        else
        {
            StartCoroutine(flashBreakDamage());
        }
    }    

    IEnumerator flashBreakDamage()
    {
        gameObject.GetComponent<Material>().color = Color.white;
        yield return new WaitForSeconds(0.15f);
        gameObject.GetComponent<Material>().color = Color.white;
    }
}
