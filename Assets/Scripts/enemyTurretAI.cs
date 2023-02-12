using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTurretAI : enemyAI
{
    void Start()
    {
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
       

    }

    public override bool canSeePlayer()
    {
        playerDirection = gameManager.instance.player.transform.position - headPos.position;
        playerDirection.y += 1;
        playerYOffset = playerDirection.y;
        //playerDirection = gameManager.instance.player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);

        //Debug.Log(angleToPlayer);
        //Debug.DrawRay(headPos.position, playerDirection);

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
                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }
}
