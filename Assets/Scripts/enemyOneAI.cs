using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemyOneAI : enemyAI
{
    [SerializeField] GameObject fireEffect;
    bool setOnFire;
    [SerializeField] GameObject iceEffect;
    bool chilled;
    bool chilledOnce;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        speedOrig = agent.speed;

        if (SceneManager.GetActiveScene().name == "LvlTwoTheArena")
        {
            gameManager.instance.updateGameGoalLvl2(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {            
            if(!chilled)
            {
                agent.speed = speedOrig;
                speedChase = speedChaseOrig;
                shootRate = shootRateOrig;
            }
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);

            if (isPlayerInRange)
            {
                if (!canSeePlayer())
                {
                    StartCoroutine(roam());
                }
            }
            else if (agent.destination != gameManager.instance.player.transform.position)
            {
                StartCoroutine(roam());
            }            
        }

    }
    
    public override void takeDamage(int dmg)
    {
        if (gameManager.instance.playerScript.fireOn && !setOnFire)
        {
            setOnFire = true;
            StartCoroutine(onFire());
        }
        else if (gameManager.instance.playerScript.iceOn && !chilled)
        {
            chilled = true;
            chilledOnce = true;
            StartCoroutine(iced());
        }
        if (dmg > 0)
        {
            hitPoints -= dmg;
        }
        if (hitPoints <= 0)
        {
            if (setOnFire)
            {
                model.material.color = Color.black;
            }
            if (chilled)
            {
                model.material.color = new Color(0, 0.5509f, 1);
                agent.enabled = false;
                GetComponent<Collider>().enabled = false;
                GetComponentInChildren<Canvas>().enabled = false;
                anim.enabled = false;
            }
            else
            {
                if (SceneManager.GetActiveScene().name == "LvlThreeTheWorld")
                {
                    gameManager.instance.updateGameGoalLvl3(-1);
                }
                GetComponent<Collider>().enabled = false;
                GetComponentInChildren<Canvas>().enabled = false;
                aud.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)], audDeathVol);
                anim.SetBool("Dead", true);
                agent.enabled = false;
            }
            if (SceneManager.GetActiveScene().name == "LvlTwoTheArena")
            {
                gameManager.instance.updateGameGoalLvl2(-1);
            }
            //Destroy(gameObject); Create a IEnumerator for destroyObject
        }
        else
        {
            anim.SetTrigger("Damage");
            if (dmg > 0)
            {
                aud.PlayOneShot(audTakeDamage[Random.Range(0, audTakeDamage.Length)], audTakeDamageVol);
            }
            // melee add a function for turning off the weapon collider.
            if (chilled && chilledOnce)
            {
                chilledOnce = false;
                agent.speed = speedOrig / 4;
                speedChase = speedChase / 4;
                shootRate = shootRate * 8;
            }
            agent.SetDestination(gameManager.instance.player.transform.position);
            StartCoroutine(flashDamage());
        }
    }
    IEnumerator onFire()
    {
        yield return new WaitForSeconds(.5f);
        fireEffect.SetActive(true);
        takeDamage(1);
        yield return new WaitForSeconds(1);        
        takeDamage(1);
        yield return new WaitForSeconds(1);
        takeDamage(1);
        yield return new WaitForSeconds(2);
        fireEffect.SetActive(false);
        setOnFire = false;
    }
    IEnumerator iced()
    {
        yield return new WaitForSeconds(.5f);
        iceEffect.SetActive(true);
        takeDamage(1);
        yield return new WaitForSeconds(1);
        takeDamage(1);
        yield return new WaitForSeconds(6);
        iceEffect.SetActive(false);
        chilled = false;
    }
}
