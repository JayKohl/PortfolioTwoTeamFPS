using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyShredder : enemyAI
{
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = gameManager.instance.player.transform.position;
        if (isPlayerInRange)
        {
            canSeePlayer();
            //StartCoroutine(shoot());
        }
    }
    public override IEnumerator shoot()
    {
        isShooting = true;

        float distanceForMelee = Vector3.Magnitude(gameManager.instance.player.transform.position - transform.position);
        //float distanceForMelee = Vector3.Distance(gameManager.instance.player.transform.position, transform.position);
        if (distanceForMelee < 2)
        {
            gameManager.instance.playerScript.takeDamage(1);
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
