using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//// test
//using UnityEngine.AI;

public class enemyBossAI : enemyAI
{
    [SerializeField] Transform shootPositionTwo;
    [SerializeField] Transform shootPositionThree;
    [SerializeField] Transform shootPositionFour;

    [SerializeField] Transform shootPositionMissile;
    [SerializeField] GameObject missile;
    public Image enemyHPBar;
    [SerializeField] int missileSpeed;
    [SerializeField] float missileShootRate;
    [SerializeField] float missileYVelocity;
    [SerializeField] float missileRange;

    [Header("----- Effects -----")]
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject plasmaExplosion;
    [SerializeField] GameObject deathFlames;
    [SerializeField] GameObject iceEffect;

    [Header("----- Audio Cont -----")]
    [SerializeField] protected AudioClip[] audMissile;
    [SerializeField] protected AudioClip[] audShield;

    [SerializeField] GameObject shield;

    [SerializeField] GameObject fuelCap;
    int enemyBossCount;
    // bool hasMelee;
    bool isMissileShoot;
    bool isShootingTwo;
    int hitPointsOrig;
    bool isHealOne;
    bool isHealTwo;
    //bool isHealThree;
    //bool isHealFour;
    bool isInCoolDown;
    bool setOnFire;
    bool chilled;
    bool chilledOnce;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.bossDead = false;
        enemyBossCount += 1;

        hitPointsOrig = hitPoints;
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;

        isHealOne = true;
        isHealTwo = true;
        //isHealThree = true;
        //isHealFour = true;
        isInCoolDown = false;

        speedOrig = agent.speed;
        speedChaseOrig = speedChase;
    }

    // Update is called once per frame
    void Update()
    {
        if (!chilled)
        {
            shootRate = shootRateOrig;
            speedChase = speedChaseOrig;
            agent.speed = speedOrig;
        }
        if (agent.isActiveAndEnabled && isInCoolDown == false)
        {            
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
            if (isPlayerInRange)
            {                
                if (!canSeePlayer())
                {
                    agent.destination = gameManager.instance.player.transform.position;
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
        //Angel Added this
        updateEnemyHPBar();
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
        if (chilled && chilledOnce)
        {
            chilledOnce = false;
            shootRate = shootRate * 8;
            agent.speed = agent.speed / 4;
            speedChase = speedChase / 4;
        }
        if (hitPoints <= 0)
        {
            gameManager.instance.lvlscript.GainExperiance(xp);

            gameManager.instance.bossDead = true;
            GetComponent<Collider>().enabled = false;
            GetComponentInChildren<Canvas>().enabled = false;
            GameObject fuel = Instantiate(fuelCap, gameObject.transform.position + new Vector3(0, 0, -5), fuelCap.transform.rotation);
            anim.SetBool("Dead", true);
            agent.enabled = false;
            aud.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)], gameManager.instance.soundVol);
            StartCoroutine(explosionTimer());
            // Destroy(gameObject);
        }
        else
        {
            if (hitPoints <= hitPointsOrig / 2)
            {
                plasmaExplosion.SetActive(true);
            }
            anim.SetTrigger("Damage");
            if (!setOnFire && !chilled)
            {
                aud.PlayOneShot(audTakeDamage[Random.Range(0, audTakeDamage.Length)], gameManager.instance.soundVol);
            }
            // meleeColliderOff();
            agent.SetDestination(gameManager.instance.player.transform.position);
            StartCoroutine(flashDamage());
        }
    }
    public IEnumerator hacking(GameObject target)
    {
        if(target == gameObject)
        {
            isInCoolDown = true;
            anim.SetTrigger("CoolDown");            
            shield.SetActive(false);
            yield return new WaitForSeconds(10);
            agentStart();
            isInCoolDown = false;
        }
    }
    IEnumerator onFire()
    {
        yield return new WaitForSeconds(.5f);
        takeDamage(1);
        yield return new WaitForSeconds(1);
        takeDamage(1);
        yield return new WaitForSeconds(1);
        takeDamage(1);
        yield return new WaitForSeconds(1);
        if (gameManager.instance.lvlbuttons.abilityDamageUp)
        {
            takeDamage(3);
        }
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
    IEnumerator explosionTimer()
    {
        explosion.SetActive(true);
        plasmaExplosion.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        explosion.SetActive(false);
        plasmaExplosion.SetActive(false);
        deathFlames.SetActive(true);        
    }
    protected override IEnumerator shoot()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
    protected IEnumerator shootTwo()
    {
        isShootingTwo = true;
        // anim.SetTrigger("ShootTwo");
        anim.SetTrigger("ShootTwo");
        yield return new WaitForSeconds(shootRate);
        isShootingTwo = false;
    }

    public override void createBullet()
    {
        GameObject bulletClone = Instantiate(bullet, shootPosition.position, bullet.transform.rotation);
        aud.PlayOneShot(audBasicAttack[Random.Range(0, audBasicAttack.Length)], gameManager.instance.soundVol);
        Vector3 shootingVector = (gameManager.instance.player.transform.position - shootPosition.position).normalized;
        bulletClone.GetComponent<Rigidbody>().velocity = shootingVector * bulletSpeed;

        GameObject bulletCloneTwo = Instantiate(bullet, shootPositionTwo.position, bullet.transform.rotation);
        aud.PlayOneShot(audBasicAttack[Random.Range(0, audBasicAttack.Length)], gameManager.instance.soundVol);
        Vector3 shootingVectorTwo = (gameManager.instance.player.transform.position - shootPositionTwo.position).normalized;
        bulletCloneTwo.GetComponent<Rigidbody>().velocity = shootingVectorTwo * bulletSpeed;

    }
    public void createBulletTwo()
    {
        GameObject bulletCloneThree = Instantiate(bullet, shootPositionThree.position, bullet.transform.rotation);
        aud.PlayOneShot(audBasicAttack[Random.Range(0, audBasicAttack.Length)], gameManager.instance.soundVol);
        Vector3 shootingVectorThree = (gameManager.instance.player.transform.position - shootPositionThree.position).normalized;
        bulletCloneThree.GetComponent<Rigidbody>().velocity = shootingVectorThree * bulletSpeed;

        GameObject bulletCloneFour = Instantiate(bullet, shootPositionFour.position, bullet.transform.rotation);
        aud.PlayOneShot(audBasicAttack[Random.Range(0, audBasicAttack.Length)], gameManager.instance.soundVol);
        Vector3 shootingVectorFour = (gameManager.instance.player.transform.position - shootPositionFour.position).normalized;
        bulletCloneFour.GetComponent<Rigidbody>().velocity = shootingVectorFour * bulletSpeed;
    }

    IEnumerator missileShoot()
    {
        isMissileShoot = true;
        createMissile();
        // anim.SetTrigger("ShootMissile");
        yield return new WaitForSeconds(missileShootRate);
        isMissileShoot = false;
    }
    public void createMissile()
    {
        playerDirection = (gameManager.instance.player.transform.position - transform.position).normalized;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);
        GameObject missileClone = Instantiate(missile, shootPositionMissile.position, missile.transform.rotation);
        aud.PlayOneShot(audMissile[Random.Range(0, audMissile.Length)], gameManager.instance.soundVol);
        Vector3 missileVector = (gameManager.instance.player.transform.position - shootPositionMissile.position).normalized;
        missileClone.GetComponent<Rigidbody>().velocity = (missileVector + new Vector3(0, missileYVelocity, 0)) * missileSpeed;
    }
    protected override bool canSeePlayer()
    {
        Vector3 two = agent.transform.position;
        Vector3 one = gameManager.instance.player.transform.position;
        float distanceToBoss = Mathf.Sqrt(Mathf.Pow((two.x - one.x), 2) + Mathf.Pow((two.y - one.y), 2) + Mathf.Pow((two.z - one.z), 2));

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
            if (isHealOne && hitPoints <= (hitPointsOrig - (hitPointsOrig * .6)))
            {
                isHealOne = false;
                agentStop();
                //agent.stoppingDistance = distanceToBoss + 5;
                StartCoroutine(coolDownEvent());
                return true;
            }
            //else if (isHealTwo && hitPoints <= (hitPointsOrig - (hitPointsOrig * .7)))
            //{
            //    isHealTwo = false;
            //    agent.stoppingDistance = distanceToBoss + 5;
            //    StartCoroutine(coolDownEvent());
            //    return true;
            //}
            else if (isHealTwo && hitPoints <= (hitPointsOrig - (hitPointsOrig * .8)))
            {
                isHealTwo = false;
                agentStop();
                //agent.stoppingDistance = distanceToBoss + 5;
                StartCoroutine(coolDownEvent());
                return true;
            }
            //else if (isHealFour && hitPoints <= (hitPointsOrig - (hitPointsOrig * .9)))
            //{
            //    isHealFour = false;
            //    agent.stoppingDistance = distanceToBoss + 5;
            //    StartCoroutine(coolDownEvent());
            //    return true;
            //}
            else if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.speed = speedChase;
                agent.SetDestination(gameManager.instance.player.transform.position);
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    facePlayer();
                }
                if (!isShooting && angleToPlayer <= shootAngle)
                {
                    StartCoroutine(shoot());
                    if (!isShootingTwo && hitPoints <= (hitPointsOrig - (hitPointsOrig * .2)))
                    {
                        StartCoroutine(shootTwo());
                    }
                }

                if (hitPoints <= (hitPointsOrig / 2))
                {
                    //Vector3 two = agent.transform.position;
                    //Vector3 one = gameManager.instance.player.transform.position;
                    //float distanceToBoss = Mathf.Sqrt(Mathf.Pow((two.x - one.x), 2) + Mathf.Pow((two.y - one.y), 2) + Mathf.Pow((two.z - one.z), 2));
                    if (!isMissileShoot && distanceToBoss >= missileRange)
                    {
                        StartCoroutine(missileShoot());
                    }
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }
    public IEnumerator coolDownEvent()
    {
        isInCoolDown = true;

        int saveStartHealth = hitPoints;
        aud.PlayOneShot(audShield[Random.Range(0, audShield.Length)], gameManager.instance.soundVol);
        shield.SetActive(true);        
        anim.SetTrigger("CoolDown");
        plasmaExplosion.SetActive(false);
        yield return new WaitForSeconds(10);
        if (saveStartHealth == hitPoints)
        {
            hitPoints += 10;
            updateEnemyHPBar();
        }
        else
        {
            hitPoints = saveStartHealth;
        }
        shield.SetActive(false);

        agentStart();
        //agent.stoppingDistance = stoppingDistOrig;
        isInCoolDown = false;
    }
    //Angel added this line
    public void updateEnemyHPBar()
    {
        enemyHPBar.fillAmount = (float)hitPoints / (float)hitPointsOrig;
    }
    //protected IEnumerator shootTwo()
    //{
    //    isShootingTwo = true;
    //    // anim.SetTrigger("ShootTwo");
    //    anim.SetTrigger("ShootTwo");
    //    yield return new WaitForSeconds(shootRate);
    //    isShootingTwo = false;
    //}

}
