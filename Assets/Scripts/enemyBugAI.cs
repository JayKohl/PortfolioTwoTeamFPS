using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class enemyBugAI : enemyAI
{
    [SerializeField] GameObject fireEffect;
    bool setOnFire;
    [SerializeField] GameObject iceEffect;
    bool chilled;
    bool chilledOnce;
    [SerializeField] protected Collider meleeColliderTwo;
    [SerializeField] Collider meleeColliderRam;
    bool chillDeath;
    [SerializeField] GameObject fracturedEffect;
    [SerializeField] AudioSource fracturedSource;
    [SerializeField] AudioClip iceBreak;

    int hitPointsOrig;
    bool isAgro;

    System.Random randomAttack;

    void Start()
    {
        fracturedSource = fracturedEffect.GetComponent<AudioSource>();
        hitPointsOrig = hitPoints;
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        speedOrig = agent.speed;
        speedChaseOrig = speedChase;
        isAgro = false;

        randomAttack = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            if (!chilled)
            {
                agent.speed = speedOrig;
                shootRate = shootRateOrig;
                speedChase = speedChaseOrig;
            }
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
            if (isPlayerInRange)
            {
                isAgro = true;
                if (!canSeePlayer())
                {
                    agent.destination = gameManager.instance.player.transform.position;
                }
            }
            else if (agent.destination != gameManager.instance.player.transform.position && isAgro == false)
            {
                StartCoroutine(roam());
            }
            else if (agent.destination != gameManager.instance.player.transform.position && isAgro == true)
            {
                agent.destination = gameManager.instance.player.transform.position;
            }
        }
    }
    protected override bool canSeePlayer()
    {
        playerDirection = (gameManager.instance.player.transform.position - shootPosition.position).normalized;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);

        //Debug.Log(angleToPlayer);
        //Debug.DrawRay(headPos.position, playerDirection);

        Vector3 two = agent.transform.position;
        Vector3 one = gameManager.instance.player.transform.position;
        float distanceToEnemy = Mathf.Sqrt(Mathf.Pow((two.x - one.x), 2) + Mathf.Pow((two.y - one.y), 2) + Mathf.Pow((two.z - one.z), 2));

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                int selectAttack = randomAttack.Next(1, 11);

                agent.stoppingDistance = stoppingDistOrig;
                agent.speed = speedChase;
                agent.SetDestination(gameManager.instance.player.transform.position);
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    facePlayer();
                }

                if (agent.isActiveAndEnabled)
                {
                    // basic attacks
                    if (!isMelee && angleToPlayer <= shootAngle && distanceToEnemy <= 7 && selectAttack <= 4)
                    {
                        StartCoroutine(melee());
                    }
                    else if (!isMelee && angleToPlayer <= shootAngle && distanceToEnemy <= 7 && selectAttack > 4 && selectAttack <= 8)
                    {
                        isMelee = true;
                        StartCoroutine(meleeTwo());
                    }
                    else if (!isMelee && angleToPlayer <= shootAngle && distanceToEnemy <= 7 && selectAttack > 8)
                    {
                        isMelee = true;
                        StartCoroutine(meleeRam());
                    }
                    return true;
                }
            }
        }
        facePlayer();
        agent.stoppingDistance = stoppingDistOrig;
        return false;
    }
    protected IEnumerator meleeRam()
    {
        //isMelee = true;
        anim.SetTrigger("Ram");
        yield return new WaitForSeconds(meleeRate);
        isMelee = false;
    }
    public void meleeRamColliderOn()
    {
        meleeColliderRam.enabled = true;
    }
    public void meleeRamColliderOff()
    {
        meleeColliderRam.enabled = false;
    }
    protected IEnumerator meleeTwo()
    {
        //isMelee = true;
        anim.SetTrigger("MeleeTwo");
        yield return new WaitForSeconds(meleeRate);
        isMelee = false;
    }
    public void meleeColliderTwoOn()
    {
        meleeColliderTwo.enabled = true;
    }
    public void meleeColliderTwoOff()
    {
        meleeColliderTwo.enabled = false;
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
            GetComponent<Collider>().enabled = false;
            model.GetComponentInChildren<Renderer>().enabled = false;
            //headPos.GetComponentInChildren<Renderer>().enabled = false;
            //shootPosition.parent.GetComponent<Renderer>().enabled = false;
            fracturedEffect.SetActive(true);
            fracturedSource.PlayOneShot(iceBreak, gameManager.instance.soundVol);
            fracturedEffect.GetComponentInChildren<ParticleSystem>().Play();
            GetComponent<Collider>().enabled = false;
            iceEffect.SetActive(false);
            StartCoroutine(death());
        }
        if (hitPoints <= 0)
        {
            gameManager.instance.lvlscript.GainExperiance(xp);
            if (SceneManager.GetActiveScene().name == "LvlThreeTheWorld")
            {
                gameManager.instance.updateGameGoalLvl3(-1);
            }
            if (setOnFire)
            {
                model.material.color = Color.black;
            }
            if (chilled)
            {
                model.material.color = new Color(0, 0.5509f, 1);
                agent.enabled = false;                
                GetComponentInChildren<Canvas>().enabled = false;
                anim.enabled = false;
                chillDeath = true;
            }
            else
            {
                GetComponent<Collider>().enabled = false;
                GetComponentInChildren<Canvas>().enabled = false;
                aud.PlayOneShot(audDeath[UnityEngine.Random.Range(0, audDeath.Length)], gameManager.instance.soundVol);
                anim.SetBool("Dead", true);
                agent.enabled = false;
            }
        }
        else
        {
            anim.SetTrigger("Damage");
            if (dmg > 0)
            {
                aud.PlayOneShot(audTakeDamage[UnityEngine.Random.Range(0, audTakeDamage.Length)], gameManager.instance.soundVol);
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
        if (gameManager.instance.lvlbuttons.abilityDamageUp)
        {
            takeDamage(3);
        }
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
