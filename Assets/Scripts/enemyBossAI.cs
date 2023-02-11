using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//// test
//using UnityEngine.AI;

public class enemyBossAI : enemyAI
{
    [SerializeField] GameObject fuelCap;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(+1);

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
                roam();
            }
        }
        else if (agent.remainingDistance < 0.1f && agent.destination != gameManager.instance.player.transform.position)
        {
            roam();
        }
    }
    public override void takeDamage(int dmg)
    {

        hitPoints -= dmg;
        StartCoroutine(flashDamage());
        if (hitPoints <= 0)
        {
            if (gameObject.CompareTag("EnemyBoss"))
            {
                GameObject fuel = Instantiate(fuelCap, gameObject.transform.position, fuelCap.transform.rotation);
                //gameManager.instance.updateGameGoal(-1);
            }
            Destroy(gameObject);
        }
    }
}
