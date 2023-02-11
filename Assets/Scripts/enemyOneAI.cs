using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyOneAI : enemyAI
{
    // Start is called before the first frame update
    void Start()
    {
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
}
