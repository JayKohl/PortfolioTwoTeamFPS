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

        roam();
    }

    // Update is called once per frame
    void Update()
    {

        if (isPlayerInRange)
        {
            if (!canSeePlayer() && agent.remainingDistance < 0.1f)
            {
                agent.destination = gameManager.instance.player.transform.position;
            }
        }
        else if (agent.remainingDistance < 0.1f && agent.destination != gameManager.instance.player.transform.position)
        {
            roam();
        }
        if (gameObject.transform.position.magnitude < stoppingDistOrig)
        {
            Destroy(gameObject);
        }
    }
    public override void takeDamage(int dmg)
    {

        hitPoints -= dmg;
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
        bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        GameObject bulletCloneTwo = Instantiate(bullet, shootPositionTwo.position, bullet.transform.rotation);
        bulletCloneTwo.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        GameObject bulletCloneThree = Instantiate(bullet, shootPositionThree.position, bullet.transform.rotation);
        bulletCloneThree.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        GameObject bulletCloneFour = Instantiate(bullet, shootPositionFour.position, bullet.transform.rotation);
        bulletCloneFour.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

}
