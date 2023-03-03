using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class endBossAI : enemyAI
{
    [SerializeField] protected Collider meleeColliderTwo;

    [SerializeField] Collider meleeColliderRam;

    [SerializeField] GameObject bulletUpgrade;
    [SerializeField] GameObject spikeUpgrade;

    [SerializeField] GameObject spike;
    [SerializeField] int spikeSpeed;
    [SerializeField] float spikeShootRate;
    [SerializeField] float spikeRange;
    [SerializeField] Transform shootPositionSpike;
    [SerializeField] Transform shootPositionSpikeTwo;
    [SerializeField] Transform shootPositionSpikeThree;
    [SerializeField] Transform shootPositionSpikeFour;
    [SerializeField] Transform shootPositionSpikeFive;
    [SerializeField] Transform shootPositionSpikeSix;
    [SerializeField] Transform shootPositionSpikeSeven;
    [SerializeField] Transform shootPositionSpikeEight;
    [SerializeField] Transform shootPositionSpikeNine;
    [SerializeField] Transform shootPositionSpikeTen;

    [SerializeField] GameObject spawnEnemyType;
    [SerializeField] Transform[] spawnPos;

    [SerializeField] GameObject smokeOne;
    [SerializeField] GameObject smokeTwo;
    public Image enemyHPBar;
    int hitPointsOrig;
    bool isEventActive;
    bool isSpikeShoot;
    bool isAgro;
    bool isMinionSpawnOne;
    bool isMinionSpawnTwo;
    bool isPowerUp;

    System.Random randomAttack;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.boss2Dead = false;
        hitPointsOrig = hitPoints;
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        speedOrig = agent.speed;

        isPowerUp = false;
        isEventActive = false;
        isAgro = false;

        randomAttack = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
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

        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDirection);

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

                if (isEventActive == false && isMinionSpawnOne == false && hitPoints <= (hitPointsOrig - (hitPointsOrig * .3)))
                {
                    isEventActive = true;
                    isMinionSpawnOne = true;
                    StartCoroutine(spawnMinions());
                }
                else if (isEventActive == false && isPowerUp == false && hitPoints <= (hitPointsOrig - (hitPointsOrig * .5)))
                {
                    isEventActive = true;
                    isPowerUp = true;
                    smokeOne.SetActive(true);
                    smokeTwo.SetActive(true);
                    StartCoroutine(powerUp());
                }
                else if (isEventActive == false && isMinionSpawnTwo == false && hitPoints <= (hitPointsOrig - (hitPointsOrig * .7)))
                {
                    isEventActive = true;
                    isMinionSpawnTwo = true;
                    StartCoroutine(spawnMinions());
                }
                else if (isEventActive == false)
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
                    else if (!isMelee && !isShooting && angleToPlayer <= shootAngle && hitPoints <= (hitPointsOrig - (hitPointsOrig * .2)))
                    {
                        StartCoroutine(shoot());
                    }
                    else if (!isSpikeShoot && distanceToEnemy >= spikeRange && hitPoints <= (hitPointsOrig - (hitPointsOrig * .2)))
                    {
                        agentStop();
                        isShooting = true;
                        isSpikeShoot = true;
                        //agent.stoppingDistance = stoppingDistOrig *= 3;
                        StartCoroutine(spikeShoot());
                    }
                    return true;
                }
            }
        }
        facePlayer();
        agent.stoppingDistance = stoppingDistOrig;
        return false;
    }
    protected IEnumerator powerUp()
    {
        anim.SetTrigger("PowerUp");
        yield return new WaitForSeconds(2);
        statsBuff();
        isEventActive = false;
    }
    public void statsBuff()
    {
        bullet = bulletUpgrade;
        spike = spikeUpgrade;
    }
    protected IEnumerator spawnMinions()
    {
        anim.SetTrigger("Spawn");
        yield return new WaitForSeconds(2);
        isEventActive = false;
    }
    public void createMinions()
    {
        Instantiate(spawnEnemyType, spawnPos[0].position, spawnPos[0].rotation);
        Instantiate(spawnEnemyType, spawnPos[1].position, spawnPos[1].rotation);
        Instantiate(spawnEnemyType, spawnPos[2].position, spawnPos[2].rotation);
        Instantiate(spawnEnemyType, spawnPos[3].position, spawnPos[3].rotation);
        Instantiate(spawnEnemyType, spawnPos[4].position, spawnPos[4].rotation);
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
    protected IEnumerator spikeShoot()
    {
        //isShooting = true;
        //isSpikeShoot = true;
        anim.SetTrigger("SpikeShoot");
        yield return new WaitForSeconds(spikeShootRate);
        //agent.stoppingDistance = stoppingDistOrig;
        isShooting = false;
        isSpikeShoot = false;
        agentStart();
    }
    public void createSpike()
    {
        GameObject spikeClone = Instantiate(spike, shootPositionSpike.position, spike.transform.rotation);
        spikeClone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        GameObject spikeCloneTwo = Instantiate(spike, shootPositionSpikeTwo.position, spike.transform.rotation);
        spikeCloneTwo.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        GameObject spikeCloneThree = Instantiate(spike, shootPositionSpikeThree.position, spike.transform.rotation);
        spikeCloneThree.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        GameObject spikeCloneFour = Instantiate(spike, shootPositionSpikeFour.position, spike.transform.rotation);
        spikeCloneFour.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        GameObject spikeCloneFive = Instantiate(spike, shootPositionSpikeFive.position, spike.transform.rotation);
        spikeCloneFive.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        GameObject spikeCloneSix = Instantiate(spike, shootPositionSpikeSix.position, spike.transform.rotation);
        spikeCloneSix.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        GameObject spikeCloneSeven = Instantiate(spike, shootPositionSpikeSeven.position, spike.transform.rotation);
        spikeCloneSeven.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        GameObject spikeCloneEight = Instantiate(spike, shootPositionSpikeEight.position, spike.transform.rotation);
        spikeCloneEight.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        GameObject spikeCloneNine = Instantiate(spike, shootPositionSpikeNine.position, spike.transform.rotation);
        spikeCloneNine.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        GameObject spikeCloneTen = Instantiate(spike, shootPositionSpikeTen.position, spike.transform.rotation);
        spikeCloneTen.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
    }
    public override void takeDamage(int dmg)
    {
        hitPoints -= dmg;
        updateEnemyHPBar();
        if (hitPoints <= 0)
        {
            
            GetComponent<Collider>().enabled = false;
            GetComponentInChildren<Canvas>().enabled = false;
            gameManager.instance.endGameTrigger.GetComponent<endGame>().endGameColliderOn();
            aud.PlayOneShot(audDeath[UnityEngine.Random.Range(0, audDeath.Length)], audDeathVol);
            anim.SetBool("Dead", true);
            agent.enabled = false;
            gameManager.instance.boss2Dead = true;
            gameManager.instance.updateGameGoalLvl2(0);
            //Destroy(gameObject); Create a IEnumerator for destroyObject
        }
        else
        {
            anim.SetTrigger("Damage");
            aud.PlayOneShot(audTakeDamage[UnityEngine.Random.Range(0, audTakeDamage.Length)], audTakeDamageVol);
            // melee add a function for turning off the weapon collider.
            agent.SetDestination(gameManager.instance.player.transform.position);
            StartCoroutine(flashDamage());
        }
    }

    public void updateEnemyHPBar()
    {
        enemyHPBar.fillAmount = (float)hitPoints / (float)hitPointsOrig;
    }
}
