using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTreeAI : enemyAI
{
    int hitPointsOrig;

    // Start is called before the first frame update
    void Start()
    {
        hitPointsOrig = hitPoints;
        agentStop();
        anim.SetTrigger("Hide");
    }

    // Update is called once per frame
    void Update()
    {
        if (canSeePlayer())
        {
            anim.SetTrigger("Idle");
            // first attack
        }
        else if (!canSeePlayer() && isPlayerInRange)
        {
            anim.SetTrigger("Idle");
            // round attack
        }
    }
    protected override bool canSeePlayer()
    {
        playerDirection = (gameManager.instance.player.transform.position - headPos.position).normalized;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);

        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDirection);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    //spin attack
                }
                if (!isMelee && angleToPlayer <= shootAngle)
                {
                    //StartCoroutine(melee());
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }
}
