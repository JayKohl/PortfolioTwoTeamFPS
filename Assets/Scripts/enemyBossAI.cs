using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//// test
//using UnityEngine.AI;

public class enemyBossAI : enemyAI
{
    [SerializeField] Transform shootPositionTwo;
    [SerializeField] Transform shootPositionThree;
    [SerializeField] Transform shootPositionFour;

    [SerializeField] Transform shootPositionMissile;
    [SerializeField] public GameObject missile;
    [SerializeField] public int missileSpeed;
    [SerializeField] public float missileShootRate;

    [SerializeField] GameObject fuelCap;
    int enemyBossCount;
    bool hasMelee;
    bool isMissileShoot;
    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(+1);
        enemyBossCount += 1;

        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        //anim.SetFloat("Speed", agent.velocity.normalized.magnitude);

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
    public override void takeDamage(int dmg)
    {

        hitPoints -= dmg;
        agent.SetDestination(gameManager.instance.player.transform.position);
        StartCoroutine(flashDamage());
        if (hitPoints <= 0)
        {
            GameObject fuel = Instantiate(fuelCap, gameObject.transform.position, fuelCap.transform.rotation);
            Destroy(gameObject);
        }
    }
    public override IEnumerator shoot()
    {
        isShooting = true;

        //GameObject bulletClone = Instantiate(bullet, shootPosition.position, bullet.transform.rotation);
        //Vector3 shootingVector = (gameManager.instance.player.transform.position - shootPosition.position).normalized;
        //bulletClone.GetComponent<Rigidbody>().velocity = shootingVector * bulletSpeed;


        GameObject bulletClone = Instantiate(bullet, shootPosition.position, bullet.transform.rotation);
        Vector3 shootingVector = (gameManager.instance.player.transform.position - shootPosition.position).normalized;
        bulletClone.GetComponent<Rigidbody>().velocity = shootingVector * bulletSpeed;

        GameObject bulletCloneTwo = Instantiate(bullet, shootPositionTwo.position, bullet.transform.rotation);
        Vector3 shootingVectorTwo = (gameManager.instance.player.transform.position - shootPositionTwo.position).normalized;
        bulletCloneTwo.GetComponent<Rigidbody>().velocity = shootingVectorTwo * bulletSpeed;

        GameObject bulletCloneThree = Instantiate(bullet, shootPositionThree.position, bullet.transform.rotation);
        Vector3 shootingVectorThree = (gameManager.instance.player.transform.position - shootPositionThree.position).normalized;
        bulletCloneThree.GetComponent<Rigidbody>().velocity = shootingVectorThree * bulletSpeed;

        GameObject bulletCloneFour = Instantiate(bullet, shootPositionFour.position, bullet.transform.rotation);
        Vector3 shootingVectorFour = (gameManager.instance.player.transform.position - shootPositionFour.position).normalized;
        bulletCloneFour.GetComponent<Rigidbody>().velocity = shootingVectorFour * bulletSpeed;

        float distanceForMelee = Vector3.Distance(gameManager.instance.player.transform.position, transform.position);
        if (distanceForMelee < 3)
        {
            gameManager.instance.playerScript.takeDamage(1);
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
    IEnumerator missileShoot()
    {
        isMissileShoot = true;

        GameObject missileClone = Instantiate(missile, shootPositionMissile.position, missile.transform.rotation);
        float shootingVectorMissile = gameManager.instance.player.transform.position.z - shootPositionMissile.position.z;

        float missileMidWay = shootingVectorMissile / 2;
        // setting the peak of the missile durring its travel
        Vector3 missileTopPos = new Vector3(gameManager.instance.transform.position.x, shootPositionMissile.transform.position.y, missileMidWay);
        Vector3 directionVecToTop = (missileTopPos - shootPositionMissile.position).normalized;
        if (missileClone.transform.position != missileTopPos)
        {
            missileClone.GetComponent<Rigidbody>().velocity = directionVecToTop * missileSpeed;
        }
        else
        {
            Vector3 missileDrop = (gameManager.instance.player.transform.position - missileTopPos);
            missileClone.GetComponent<Rigidbody>().velocity = missileDrop * missileSpeed;
        }

        yield return new WaitForSeconds(missileShootRate);
        isMissileShoot = false;
    }
    public override bool canSeePlayer()
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
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    facePlayer();
                }
                if (!isShooting && angleToPlayer <= shootAngle)
                {
                    StartCoroutine(shoot());
                }
                if (!isMissileShoot)
                {
                    StartCoroutine(missileShoot());
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

}
