using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyMechAI : enemyAI
{
    [SerializeField] Transform shootPositionTwo;
    [SerializeField] Transform shootPositionThree;
    [SerializeField] Transform shootPositionFour;

    [SerializeField] Transform shootPositionMissile;
    [SerializeField] GameObject missile;
    [SerializeField] int missileSpeed;
    [SerializeField] float missileShootRate;
    [SerializeField] float missileYVelocity;
    [SerializeField] float missileRange;

    [Header("----- Effects -----")]
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject plasmaExplosion;
    [SerializeField] GameObject deathFlames;

    [Header("----- Audio Cont -----")]
    [SerializeField] protected AudioClip[] audMissile;

    bool isMissileShoot;
    bool isShootingTwo;
    int hitPointsOrig;

    int airTime;
    Vector3 one;
    Vector3 two;
    float distanceToBoss;


    void Start()
    {
        hitPointsOrig = hitPoints;
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        speedOrig = agent.speed;
    }

    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
            if (isPlayerInRange)
            {
                two = agent.transform.position;
                one = gameManager.instance.player.transform.position;
                distanceToBoss = Mathf.Sqrt(Mathf.Pow((two.x - one.x), 2) + Mathf.Pow((two.y - one.y), 2) + Mathf.Pow((two.z - one.z), 2));
                if (agent.transform.position.y < gameManager.instance.player.transform.position.y)
                {
                    airTime++;
                }
                else
                {
                    airTime = 0;
                }
                if (airTime < 180 || distanceToBoss > 8)
                {
                    if (!canSeePlayer())
                    {
                        agent.destination = gameManager.instance.player.transform.position;
                    }
                }
            }
            else if (agent.destination != gameManager.instance.player.transform.position)
            {
                StartCoroutine(roam());
            }
        }
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
        aud.PlayOneShot(audBasicAttack[Random.Range(0, audBasicAttack.Length)]);
        Vector3 shootingVector = (gameManager.instance.player.transform.position - shootPosition.position).normalized;
        bulletClone.GetComponent<Rigidbody>().velocity = shootingVector * bulletSpeed;

        GameObject bulletCloneTwo = Instantiate(bullet, shootPositionTwo.position, bullet.transform.rotation);
        aud.PlayOneShot(audBasicAttack[Random.Range(0, audBasicAttack.Length)]);
        Vector3 shootingVectorTwo = (gameManager.instance.player.transform.position - shootPositionTwo.position).normalized;
        bulletCloneTwo.GetComponent<Rigidbody>().velocity = shootingVectorTwo * bulletSpeed;

    }
    public void createBulletTwo()
    {
        GameObject bulletCloneThree = Instantiate(bullet, shootPositionThree.position, bullet.transform.rotation);
        aud.PlayOneShot(audBasicAttack[Random.Range(0, audBasicAttack.Length)]);
        Vector3 shootingVectorThree = (gameManager.instance.player.transform.position - shootPositionThree.position).normalized;
        bulletCloneThree.GetComponent<Rigidbody>().velocity = shootingVectorThree * bulletSpeed;

        GameObject bulletCloneFour = Instantiate(bullet, shootPositionFour.position, bullet.transform.rotation);
        aud.PlayOneShot(audBasicAttack[Random.Range(0, audBasicAttack.Length)]);
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
        aud.PlayOneShot(audMissile[Random.Range(0, audMissile.Length)]);
        Vector3 missileVector = (gameManager.instance.player.transform.position - shootPositionMissile.position).normalized;
        missileClone.GetComponent<Rigidbody>().velocity = (missileVector + new Vector3(0, missileYVelocity, 0)) * missileSpeed;
    }
    protected override bool canSeePlayer()
    {
        //Vector3 two = agent.transform.position;
        //Vector3 one = gameManager.instance.player.transform.position;
        //float distanceToBoss = Mathf.Sqrt(Mathf.Pow((two.x - one.x), 2) + Mathf.Pow((two.y - one.y), 2) + Mathf.Pow((two.z - one.z), 2));

        playerDirection = (gameManager.instance.player.transform.position - headPos.position).normalized;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);

        //Debug.Log(angleToPlayer);
        //Debug.DrawRay(headPos.position, playerDirection);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
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
}
