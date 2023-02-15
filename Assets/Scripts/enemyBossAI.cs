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
    [SerializeField] GameObject fuelCap;
    int enemyBossCount;
    bool hasMelee;
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

        GameObject bulletClone = Instantiate(bullet, shootPosition.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = playerDirection * bulletSpeed;

        GameObject bulletCloneTwo = Instantiate(bullet, shootPositionTwo.position, bullet.transform.rotation);
        bulletCloneTwo.GetComponent<Rigidbody>().velocity = playerDirection * bulletSpeed;

        GameObject bulletCloneThree = Instantiate(bullet, shootPositionThree.position, bullet.transform.rotation);
        bulletCloneThree.GetComponent<Rigidbody>().velocity = playerDirection * bulletSpeed;

        GameObject bulletCloneFour = Instantiate(bullet, shootPositionFour.position, bullet.transform.rotation);
        bulletCloneFour.GetComponent<Rigidbody>().velocity = playerDirection * bulletSpeed;

        float distanceForMelee = Vector3.Distance(gameManager.instance.player.transform.position, transform.position);
        if (distanceForMelee < 3)
        {
            gameManager.instance.playerScript.takeDamage(1);
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

}
