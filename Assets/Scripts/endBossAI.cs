using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class endBossAI : enemyShredder
{
    [SerializeField] protected Collider meleeColliderTwo;

    System.Random randomAttack;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        speedOrig = agent.speed;

        randomAttack = new System.Random();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
    protected override bool canSeePlayer()
    {
        playerDirection = (gameManager.instance.player.transform.position - headPos.position).normalized;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);

        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDirection);

        Vector3 two = agent.transform.position;
        Vector3 one = gameManager.instance.player.transform.position;
        float distanceToEnemy = Mathf.Sqrt(Mathf.Pow((two.x - one.x), 2) + Mathf.Pow((two.y - one.y), 2) + Mathf.Pow((two.z - one.z), 2));

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                int selectAttack = randomAttack.Next(1, 11);

                agent.stoppingDistance = stoppingDistOrig;
                agent.speed = speedChase;
                agent.SetDestination(gameManager.instance.player.transform.position);
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    facePlayer();
                }
                if (!isMelee && angleToPlayer <= shootAngle && distanceToEnemy <= 7 && selectAttack < 6)
                {
                    StartCoroutine(melee());
                }
                if (!isMelee && angleToPlayer <= shootAngle && distanceToEnemy <= 7 && selectAttack >= 6)
                {
                    StartCoroutine(meleeTwo());
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }
    protected IEnumerator meleeTwo()
    {
        isMelee = true;
        anim.SetTrigger("MeleeTwo");
        yield return new WaitForSeconds(meleeRate);
        isMelee = false;
    }
    public void meleeColliderTwoOn()
    {
        meleeColliderTwo.enabled = true;
    }
    public void meleeColliderTwoOff()
    {
        meleeColliderTwo.enabled = false;
    }
}
