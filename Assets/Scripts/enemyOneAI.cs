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
    }

    // Update is called once per frame
    void Update()
    {
        //anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
        if (isPlayerInRange)
        {
            if (!canSeePlayer())
            {
                StartCoroutine(roam());
            }
        }
        else if (agent.destination != gameManager.instance.player.transform.position)
        {
            StartCoroutine(roam());
        }
        
    }
}
