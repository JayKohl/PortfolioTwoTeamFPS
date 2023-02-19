using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class endBossAI : enemyShredder
{
    [SerializeField] protected Collider meleeColliderTwo;

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

    bool isSpikeShoot;

    System.Random randomAttack;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        speedOrig = agent.speed;

        randomAttack = new System.Random();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
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
                if (!isMelee && angleToPlayer <= shootAngle && distanceToEnemy <= 7 && selectAttack < 6)
                {
                    StartCoroutine(melee());
                }
                if (!isMelee && angleToPlayer <= shootAngle && distanceToEnemy <= 7 && selectAttack >= 6)
                {
                    StartCoroutine(meleeTwo());
                }
                if (!isMelee && !isShooting && angleToPlayer <= shootAngle)
                {
                    StartCoroutine(shoot());
                }
                if (!isSpikeShoot && distanceToEnemy >= spikeRange)
                {
                    agent.stoppingDistance = stoppingDistOrig *= 3;
                    StartCoroutine(spikeShoot());
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }
    protected IEnumerator meleeTwo()
    {
        isMelee = true;
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
        isShooting = true;
        isSpikeShoot = true;
        anim.SetTrigger("SpikeShoot");
        yield return new WaitForSeconds(spikeShootRate);
        agent.stoppingDistance = stoppingDistOrig;
        isShooting = false;
        isSpikeShoot = false;
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
}
