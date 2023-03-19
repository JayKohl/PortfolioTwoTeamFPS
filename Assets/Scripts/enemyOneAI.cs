using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemyOneAI : enemyAI
{
    [Header("----- Effects -----")]
    [SerializeField] GameObject fireEffect;
    bool setOnFire;
    [SerializeField] GameObject iceEffect;
    bool chilled;
    bool chilledOnce;
    bool chillDeath;
    [SerializeField] GameObject fracturedEffect;
    [SerializeField] AudioSource fracturedSource;
    [SerializeField] AudioClip iceBreak;

    // Start is called before the first frame update
    void Start()
    {
        fracturedSource = fracturedEffect.GetComponent<AudioSource>();
        speedChaseOrig = speedChase;
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
            if (!chilled)
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
        if (chillDeath && anim.enabled == false && hitPoints < -3)
        {            
            model.GetComponentInChildren<Renderer>().enabled = false;
            if (headPos.GetComponentInChildren<Renderer>() != null)
            {
                headPos.GetComponentInChildren<Renderer>().enabled = false;
            }
            if (shootPosition.parent.GetComponent<Renderer>() != null)
            {
                shootPosition.parent.GetComponent<Renderer>().enabled = false;
            }
            if (shootPosition.parent.GetComponentInChildren<Renderer>() != null)
            {
                shootPosition.parent.gameObject.SetActive(false);
            }
            fracturedEffect.SetActive(true);
            fracturedSource.PlayOneShot(iceBreak, gameManager.instance.soundVol);
            fracturedEffect.GetComponentInChildren<ParticleSystem>().Play();            
            GetComponent<Collider>().enabled = false;
            iceEffect.SetActive(false);
            StartCoroutine(death());
        }
        if (hitPoints <= 0)
        {
            if(!chilled)
            {
                iceEffect.SetActive(false);
            }
            else
            {
                iceEffect.SetActive(true);
            }
            if (SceneManager.GetActiveScene().name == "LvlTwoTheArena")
            {
                gameManager.instance.updateGameGoalLvl2(-1);
            }
            else if (SceneManager.GetActiveScene().name == "LvlThreeTheWorld")
            {
                gameManager.instance.updateGameGoalLvl3(-1);
            }
            gameManager.instance.lvlscript.GainExperiance(xp);
            if (setOnFire)
            {
                model.material.color = Color.black;
            }
            if (chilled)
            {
                model.material.color = new Color(0, 0.5509f, 1);
                GetComponentInChildren<Canvas>().enabled = false;
                anim.enabled = false;
                agent.enabled = false;
                chillDeath = true;
            }
            else
            {
                //if (SceneManager.GetActiveScene().name == "LvlThreeTheWorld")
                //{
                //    gameManager.instance.updateGameGoalLvl3(-1);
                //}
                GetComponent<Collider>().enabled = false;
                GetComponentInChildren<Canvas>().enabled = false;
                if (agent.enabled == true)
                {
                    aud.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)], gameManager.instance.soundVol);
                    anim.SetBool("Dead", true);
                    agent.enabled = false;
                }
            }
            //Destroy(gameObject); Create a IEnumerator for destroyObject
        }
        else
        {
            anim.SetTrigger("Damage");
            if (dmg > 0)
            {
                aud.PlayOneShot(audTakeDamage[Random.Range(0, audTakeDamage.Length)], gameManager.instance.soundVol);
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
        if(gameManager.instance.lvlbuttons.abilityDamageUp)
        {
            takeDamage(3);
        }
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
        if (gameManager.instance.lvlbuttons.abilityDamageUp)
        {
            takeDamage(2);
        }
        iceEffect.SetActive(false);
        chilled = false;
    }
    IEnumerator death()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
