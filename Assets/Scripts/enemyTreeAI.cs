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
        hitPointsOrig = hitPoints;
        isFirstTime = true;
        hitPointsOrig = hitPoints;
        anim.SetTrigger("UnderGround");
        //agentStop();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange == true && isFirstTime == true)
        {
            isFirstTime = false;
            anim.SetTrigger("sprout");
            StartCoroutine(sprout());
        }
        else
        {
            canSeePlayer();
        }
        //else if (isFirstTime == false)
        //{
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
                int randomAttack = Random.Range(1, 2);
                if (!isMelee && angleToPlayer <= shootAngle && randomAttack == 1)
                {
                    // single melee attack
                    StartCoroutine(melee());
                }
                else if (!isMelee && angleToPlayer <= shootAngle && randomAttack == 2)
                {
                    StartCoroutine(meleeMulti());
                }
                return true;
            }
        }
        if (!isMelee && angleToPlayer > shootAngle)
        {
            StartCoroutine(meleeSpin());
        }
        agent.stoppingDistance = 0;
        return false;
    }
    protected IEnumerator meleeMulti()
    {
        isMelee = true;
        anim.SetTrigger("MultiMelee");
        //aud.PlayOneShot(audBasicAttack[Random.Range(0, audBasicAttack.Length)], audBasicAttackVol);
        yield return new WaitForSeconds(meleeRate);
        isMelee = false;
    }
    protected IEnumerator meleeSpin()
    {
        isMelee = true;
        anim.SetTrigger("SpinMelee");
        //aud.PlayOneShot(audBasicAttack[Random.Range(0, audBasicAttack.Length)], audBasicAttackVol);
        yield return new WaitForSeconds(meleeRate);
        isMelee = false;
    }
}
