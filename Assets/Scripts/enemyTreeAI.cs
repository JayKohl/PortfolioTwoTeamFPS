using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTreeAI : enemyAI
{
    int hitPointsOrig;
    bool isFirstTime;

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.SetActive(false);
        //gameObject.transform.Translate(0f, -7f, 0f, Space.Self);
        isFirstTime = true;
        hitPointsOrig = hitPoints;
        anim.SetTrigger("UnderGround");
        //agentStop();
        //anim.SetTrigger("Hide");
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isPlayerInRange && !isFirstTime)
        //{
        //}
        if (isPlayerInRange == true && isFirstTime == true)
        {
            //gameObject.SetActive(true);
            isFirstTime = false;
            anim.SetTrigger("sprout");
            StartCoroutine(sprout());
        }
        //else if (isFirstTime == false)
        //{

        //    //anim.SetFloat("IdleSpeed", 1f);
        //    if (canSeePlayer())
        //    {
        //        //anim.SetBool("IsNotInGround", true);
        //        //anim.SetTrigger("Idle");
        //        // first attack
        //    }
        //    else if (!canSeePlayer() && isPlayerInRange)
        //    {
        //        //anim.SetTrigger("Idle");
        //        // round attack
        //    }
        //}
    }
    IEnumerator sprout()
    {
        anim.SetTrigger("OutOfGround");
        yield return new WaitForSeconds(1.1f);

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
