using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemyTurretAI : enemyAI
{
    [SerializeField] Transform shootPositionTwo;
    [SerializeField] Transform shootPositionThree;
    [SerializeField] Transform shootPositionFour;

    [Header("----- Effects -----")]
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject plasmaExplosion;
    [SerializeField] GameObject deathFlames;
    [SerializeField] GameObject iceEffect;
    bool setOnFire;
    bool chilled;
    bool chilledOnce;
    bool isInCoolDown;
    // Make sure the NavMesh stopping distance is the same as the sphere collider trigger radius.    

    bool alive;
    void Start()
    {
        alive = true;
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        speedOrig = agent.speed;
        isInCoolDown = false;
    }

    //// Update is called once per frame
    void Update()
    {        
        if (isPlayerInRange && alive)
        {
            if (!chilled)
            {
                shootRate = shootRateOrig;
            }
            canSeePlayer();
        }
    }
    protected override bool canSeePlayer()
    {
        playerDirection = (gameManager.instance.player.transform.position - headPos.position).normalized;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);

        //Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDirection);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                if (isPlayerInRange)
                {
                    facePlayer();
                }
                if (!isShooting && angleToPlayer <= shootAngle)
                {
                    StartCoroutine(shoot());
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

    protected override void facePlayer()
    {
        if (!isInCoolDown)
        {
            Quaternion rotate = Quaternion.LookRotation(playerDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.deltaTime * playerFaceSpeed);
        }
    }
    protected override IEnumerator shoot()
    {
        if (!isInCoolDown)
        {
            isShooting = true;

            GameObject bulletClone = Instantiate(bullet, shootPosition.position, bullet.transform.rotation);
            aud.PlayOneShot(audBasicAttack[Random.Range(0, audBasicAttack.Length)], audBasicAttackVol);
            Vector3 shootingVector = (gameManager.instance.player.transform.position - shootPosition.position).normalized;
            bulletClone.GetComponent<Rigidbody>().velocity = shootingVector * bulletSpeed;
            if (shootPositionTwo != null)
            {
                GameObject bulletCloneTwo = Instantiate(bullet, shootPositionTwo.position, bullet.transform.rotation);
                aud.PlayOneShot(audBasicAttack[Random.Range(0, audBasicAttack.Length)], audBasicAttackVol);
                Vector3 shootingVectorTwo = (gameManager.instance.player.transform.position - shootPositionTwo.position).normalized;
                bulletCloneTwo.GetComponent<Rigidbody>().velocity = shootingVectorTwo * bulletSpeed;

                GameObject bulletCloneThree = Instantiate(bullet, shootPositionThree.position, bullet.transform.rotation);
                aud.PlayOneShot(audBasicAttack[Random.Range(0, audBasicAttack.Length)], audBasicAttackVol);
                Vector3 shootingVectorThree = (gameManager.instance.player.transform.position - shootPositionThree.position).normalized;
                bulletCloneThree.GetComponent<Rigidbody>().velocity = shootingVectorThree * bulletSpeed;

                GameObject bulletCloneFour = Instantiate(bullet, shootPositionFour.position, bullet.transform.rotation);
                aud.PlayOneShot(audBasicAttack[Random.Range(0, audBasicAttack.Length)], audBasicAttackVol);
                Vector3 shootingVectorFour = (gameManager.instance.player.transform.position - shootPositionFour.position).normalized;
                bulletCloneFour.GetComponent<Rigidbody>().velocity = shootingVectorFour * bulletSpeed;
            }

            yield return new WaitForSeconds(shootRate);
            isShooting = false;
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
        if (chilled && chilledOnce)
        {
            chilledOnce = false;
            shootRate = shootRate * 8;
        }
        StartCoroutine(flashDamage());
        if (hitPoints <= 0)
        {
            gameManager.instance.lvlscript.GainExperiance(xp);
            if (SceneManager.GetActiveScene().name == "LvlThreeTheWorld")
            {
                gameManager.instance.updateGameGoalLvl3(-1);
            }
            GetComponent<Collider>().enabled = false;
            GetComponentInChildren<Canvas>().enabled = false;
            if (!setOnFire)
            {
                aud.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)], audDeathVol);
            }
            agent.enabled = false;
            alive = false;
            StartCoroutine(die());
        }
        else
        {
            if (dmg > 0)
            {
                aud.PlayOneShot(audTakeDamage[Random.Range(0, audTakeDamage.Length)], audTakeDamageVol);
            }
        }
    }
    IEnumerator die()
    {        
        explosion.SetActive(true);
        plasmaExplosion.SetActive(true);
        deathFlames.SetActive(true);
        yield return new WaitForSeconds(2);
        explosion.SetActive(false);
        plasmaExplosion.SetActive(false);        
    }
    public IEnumerator hacking(GameObject target)
    {
        if (target == gameObject)
        {
            isInCoolDown = true;
            yield return new WaitForSeconds(10);
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
        if (gameManager.instance.lvlMenu.GetComponent<LVLButtons>().abilityDamageUp)
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
        if (gameManager.instance.lvlMenu.GetComponent<LVLButtons>().abilityDamageUp)
        {
            takeDamage(2);
        }
            yield return new WaitForSeconds(6);
        iceEffect.SetActive(false);
        chilled = false;
    }
}
