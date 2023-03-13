using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTreeAI : enemyAI
{
    [SerializeField] protected Renderer modelNoHit;
    int hitPointsOrig;
    bool isFirstTime;
    bool isSprouting;

    bool setOnFire;
    bool chilled;
    bool chilledOnce;
    bool isInCoolDown;

    // Start is called before the first frame update
    void Start()
    {
        //agentStop();
        isSprouting = true;
        hitPointsOrig = hitPoints;
        isFirstTime = true;
        hitPointsOrig = hitPoints;
        anim.SetTrigger("UnderGround");
        //agentStop();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("Dead") == false)
        {
            if (!chilled)
            {
                shootRate = shootRateOrig;
            }
            if (isPlayerInRange == true && isFirstTime == true)
            {
                isFirstTime = false;
                anim.SetTrigger("sprout");
                StartCoroutine(sprout());
            }
            else if (isPlayerInRange == true && isFirstTime == false && isSprouting == false)
            {
                canSeePlayer();
            }
        }
    }
    IEnumerator sprout()
    {
        anim.SetTrigger("OutOfGround");
        yield return new WaitForSeconds(2f);
        isSprouting = false;
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
                int randomAttack = Random.Range(0, 2);
                if (!isMelee && angleToPlayer <= shootAngle && randomAttack == 0)
                {
                    StartCoroutine(melee());
                }
                else if (!isMelee && angleToPlayer <= shootAngle && randomAttack == 1)
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
        //aud.PlayOneShot(audBasicAttack[Random.Range(0, audBasicAttack.Length)], gameManager.instance.soundVol);
        yield return new WaitForSeconds(meleeRate);
        isMelee = false;
    }
    protected IEnumerator meleeSpin()
    {
        isMelee = true;
        anim.SetTrigger("SpinMelee");
        //aud.PlayOneShot(audBasicAttack[Random.Range(0, audBasicAttack.Length)], gameManager.instance.soundVol);
        yield return new WaitForSeconds(meleeRate);
        isMelee = false;
    }
}
