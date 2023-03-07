using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyThirdBoss : enemyAI
{
    [SerializeField] GameObject topPiece;
    [SerializeField] GameObject takeDamFX;
    [SerializeField] GameObject activeFX;
    [SerializeField] GameObject lockDownFX;
    [SerializeField] GameObject pyramidBase;

    [SerializeField] protected GameObject bulletSecond;
    [SerializeField] protected GameObject bulletThird;
    [SerializeField] protected GameObject bulletFourth;
    [SerializeField] protected GameObject bulletFifth;
    [SerializeField] protected GameObject bulletSixth;
    [SerializeField] GameObject shield;

    [SerializeField] GameObject[] spawnEnemyType;
    [SerializeField] GameObject[] mechanicalTypeEnemies;
    [SerializeField] GameObject[] bugTypeEnemies;
    [SerializeField] Transform[] spawnPos;

    bool takeDamFXDelay;
    [SerializeField] GameObject deathFX;
    bool isFlip;
    bool isReFlip;
    bool isReDrop;
    bool isSpawnEvent;
    int hitPointsOrig;
    int fullFlip;
    int fullReFlip;
    int fullDrop;
    int fullReDrop;
    bool isUpdateGameGoal;
    bool isGoingBackUp;

    bool waveOne;
    bool waveTwoBoss;
    bool waveThree;
    bool waveFourBoss;
    bool waveFive;
    int waveCount;

    // Start is called before the first frame update
    void Start()
    {
        isReFlip = false;
        isReDrop = false;

        isGoingBackUp = false;


        isUpdateGameGoal = false;
        hitPointsOrig = hitPoints;
        agentStop();
        isFlip = false;
        takeDamFXDelay = false;
        Vector3 StartingPos = topPiece.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        // hit phase
        if (!isFlip && !isSpawnEvent)
        {
            activeFX.SetActive(true);
            if (!canSeePlayer())
            {
                shield.SetActive(true);
            }
            else
            {
                shield.SetActive(false);
            }
            //canSeePlayer();
            topPiece.transform.Rotate(0f, 1f, 0f, Space.Self);
        }
        // No hit and spawn phase
        else if (isFlip && isSpawnEvent)
        {
            activeFX.SetActive(false);
            topPiece.transform.Rotate(.5f, 0f, 0f);
            fullFlip++;
        }
        if (isSpawnEvent)
        {

            // Full flip of object
            if (fullFlip >= 360)
            {
                topPiece.transform.Translate(0f, -.01f, 0f);
                fullDrop++;
                isFlip = false;
                // Moved all the way down.
                if (fullDrop >= 360)
                {
                    lockDownFX.SetActive(true);
                    //model.material.color = Color.blue;
                    spawnWave();
                    fullFlip = 0;
                    fullDrop = 0;
                    isUpdateGameGoal = true;
                    isGoingBackUp = true;
                }
            }

            if (isGoingBackUp)
            {
                if (gameManager.instance.enemiesRemaining <= 0 && isReDrop == false)
                {
                    topPiece.transform.Translate(0f, 0.01f, 0f);
                    fullReDrop++;
                    if (fullReDrop >= 350)
                    {
                        isReDrop = true;
                        fullReDrop = 0;
                        //isGoingBackUp = false;
                    }
                }
                else if (isReFlip == false && isReDrop == true)
                {
                    topPiece.transform.Rotate(-.5f, 0f, 0f);
                    fullReFlip++;
                    if (fullReFlip >= 350)
                    {
                        //isReFlip = true;
                        isReDrop = false;
                        fullReFlip = 0;
                        isGoingBackUp = false;
                        isUpdateGameGoal = false;
                        topPiece.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                        GetComponent<Collider>().enabled = true;

                        //re set to normal non event behavior.
                        lockDownFX.SetActive(false);
                        isFlip = false;
                        isSpawnEvent = false;
                    }
                }
            }
            //else if (isGoingBackUp == false && isReFlip == false)
            //{
            //    topPiece.transform.Rotate(-.5f, 0f, 0f);
            //    fullReFlip++;
            //    if (fullReFlip >= 350)
            //    {
            //        isReFlip = true;
            //        fullReFlip = 0;
            //        //isReFlip = true;
            //    }
            //}


            //topPiece.transform.Rotate(-.5f, 0f, 0f);
            //fullReFlip++;
            //if (fullReFlip >= 350)
            //{
            //    isReFlip = true;
            //    fullReFlip = 0;
            //}
        }






        //else if (gameManager.instance.enemiesRemaining <= 0 && isGoingBackUp)
        //{
        //    topPiece.transform.Translate(0f, .01f, 0f);
        //    fullDrop++;
        //    if (fullDrop >= 360)
        //    {
        //        isGoingBackUp = false;
        //        fullDrop = 0;
        //    }
        //}


        //if (gameManager.instance.enemiesRemaining <= 0 && isSpawnEvent && isUpdateGameGoal == true && isGoingBackUp == false)
        //{
        //}
        //if (isSpawnEvent)
        //{
        //    isSpawnEvent = false;
        //    model.material.color = Color.blue;
        //}
    }
    public override void takeDamage(int dmg)
    {
        hitPoints -= dmg;
        if (hitPoints <= 0)
        {
            GetComponent<Collider>().enabled = false;
            //GetComponentInChildren<Canvas>().enabled = false;
            //aud.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)], audDeathVol);
            agent.enabled = false;
            deathFX.SetActive(true);
            StartCoroutine(deathDestroy());
        }
        else
        {
            //anim.SetTrigger("Damage");
            if (dmg > 0)
            {
                aud.PlayOneShot(audTakeDamage[Random.Range(0, audTakeDamage.Length)], audTakeDamageVol);
            }
            if (!isSpawnEvent && takeDamFXDelay == false)
            {
                takeDamFXDelay = true;
                takeDamFX.SetActive(true);
                StartCoroutine(onDamageFX());
                StartCoroutine(flashDamage());
            }
        }
        // events
        if (isSpawnEvent == false && waveOne == false && hitPoints <= (hitPointsOrig - (hitPointsOrig * .1)))
        {
            startingEvent();
            waveOne = !waveOne;
        }
        else if (isSpawnEvent == false && waveTwoBoss == false && hitPoints <= (hitPointsOrig - (hitPointsOrig * .3)))
        {
            startingEvent();
            waveTwoBoss = !waveTwoBoss;
        }
        else if (isSpawnEvent == false && waveThree == false && hitPoints <= (hitPointsOrig - (hitPointsOrig * .5)))
        {
            startingEvent();
            waveThree = !waveThree;
        }
        else if (isSpawnEvent == false && waveFourBoss == false && hitPoints <= (hitPointsOrig - (hitPointsOrig * .7)))
        {
            startingEvent();
            waveFourBoss = !waveFourBoss;
        }
        else if (isSpawnEvent == false && waveFive == false && hitPoints <= (hitPointsOrig - (hitPointsOrig * .9)))
        {
            startingEvent();
            waveFive = !waveFive;
        }
    }
    private void startingEvent()
    {
        waveCount++;
        GetComponent<Collider>().enabled = false;
        isSpawnEvent = true;
        isFlip = true;
        topPiece.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
    }
    IEnumerator deathDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    IEnumerator onDamageFX()
    {
        yield return new WaitForSeconds(.5f);
        takeDamFX.SetActive(false);
        takeDamFXDelay = false;
    }
    protected override bool canSeePlayer()
    {
        playerDirection = (gameManager.instance.player.transform.position - headPos.position).normalized;
        // playerDirection.y += 1;
        //playerYOffset = playerDirection.y;
        //playerDirection = gameManager.instance.player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);

        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDirection);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player"))// && angleToPlayer <= viewAngle)
            {
                //agent.stoppingDistance = stoppingDistOrig;
                //agent.speed = speedChase;
                //agent.SetDestination(gameManager.instance.player.transform.position);
                //if (agent.remainingDistance < agent.stoppingDistance)
                //{
                //    //facePlayer();
                //}
                if (!isShooting) //&& angleToPlayer <= shootAngle)
                {
                    StartCoroutine(shoot());
                }
                return true;
            }
        }
        //agent.stoppingDistance = 0;
        return false;
    }
    protected override IEnumerator shoot()
    {
        isShooting = true;
        if (waveCount == 1)
        {
            bullet = bulletSecond;
            //bulletSpeed = (bulletSpeed + (bulletSpeed / 2));
            //shootRate = (shootRate - .05f);
        }
        else if (waveCount == 2)
        {
            bullet = bulletThird;
        }
        else if (waveCount == 3)
        {
            bullet = bulletFourth;
        }
        else if (waveCount == 4)
        {
            bullet = bulletFifth;
        }
        else if (waveCount == 5)
        {
            bullet = bulletSixth;
        }
        createBullet();
        //anim.SetTrigger("Shoot");
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
    public override void createBullet()
    {
        if (!blind)
        {
            GameObject bulletClone = Instantiate(bullet, shootPosition.position, bullet.transform.rotation);
            bulletClone.GetComponent<Rigidbody>().velocity = playerDirection * bulletSpeed;
        }
        else
        {
            GameObject bulletClone = Instantiate(bullet, shootPosition.position, bullet.transform.rotation);
            bulletClone.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(0, 0.5f), Random.Range(0, 0.5f), Random.Range(0, 0.5f)) * bulletSpeed;
        }
        //aud.PlayOneShot(audBasicAttack[Random.Range(0, audBasicAttack.Length)], audBasicAttackVol);
    }
    protected void spawnWave()
    {
        if (waveCount == 1 || waveCount == 3 || waveCount == 5)
        {
            Instantiate(spawnEnemyType[Random.Range(0, spawnEnemyType.Length)], spawnPos[0].position, spawnPos[0].rotation);
            Instantiate(spawnEnemyType[Random.Range(0, spawnEnemyType.Length)], spawnPos[1].position, spawnPos[1].rotation);
            Instantiate(spawnEnemyType[Random.Range(0, spawnEnemyType.Length)], spawnPos[2].position, spawnPos[2].rotation);
            Instantiate(spawnEnemyType[Random.Range(0, spawnEnemyType.Length)], spawnPos[3].position, spawnPos[3].rotation);
            Instantiate(spawnEnemyType[Random.Range(0, spawnEnemyType.Length)], spawnPos[4].position, spawnPos[4].rotation);
        }
        else if (waveCount == 2)
        {
            Instantiate(bugTypeEnemies[Random.Range(0, bugTypeEnemies.Length)], spawnPos[0].position, spawnPos[0].rotation);
            Instantiate(bugTypeEnemies[Random.Range(0, bugTypeEnemies.Length)], spawnPos[1].position, spawnPos[1].rotation);
            Instantiate(bugTypeEnemies[Random.Range(0, bugTypeEnemies.Length)], spawnPos[2].position, spawnPos[2].rotation);
            Instantiate(bugTypeEnemies[Random.Range(0, bugTypeEnemies.Length)], spawnPos[3].position, spawnPos[3].rotation);
            Instantiate(bugTypeEnemies[Random.Range(0, bugTypeEnemies.Length)], spawnPos[4].position, spawnPos[4].rotation);
        }
        else if (waveCount == 4)
        {
            Instantiate(mechanicalTypeEnemies[Random.Range(0, mechanicalTypeEnemies.Length)], spawnPos[0].position, spawnPos[0].rotation);
            Instantiate(mechanicalTypeEnemies[Random.Range(0, mechanicalTypeEnemies.Length)], spawnPos[1].position, spawnPos[1].rotation);
            Instantiate(mechanicalTypeEnemies[Random.Range(0, mechanicalTypeEnemies.Length)], spawnPos[2].position, spawnPos[2].rotation);
            Instantiate(mechanicalTypeEnemies[Random.Range(0, mechanicalTypeEnemies.Length)], spawnPos[3].position, spawnPos[3].rotation);
            Instantiate(mechanicalTypeEnemies[Random.Range(0, mechanicalTypeEnemies.Length)], spawnPos[4].position, spawnPos[4].rotation);
        }
        gameManager.instance.updateGameGoalLvl3(5);
    }
}
