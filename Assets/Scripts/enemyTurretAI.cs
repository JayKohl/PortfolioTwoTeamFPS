using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTurretAI : enemyAI
{
    [SerializeField] Transform shootPositionTwo;
    [SerializeField] Transform shootPositionThree;
    [SerializeField] Transform shootPositionFour;
    // Make sure the NavMesh stopping distance is the same as the sphere collider trigger radius.
    void Start()
    {
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }

    //// Update is called once per frame
    void Update()
    {
        if (isPlayerInRange)
        {
            canSeePlayer();
        }
    }

    public override bool canSeePlayer()
    {
        playerDirection = (gameManager.instance.player.transform.position - headPos.position).normalized;
        playerDirection.y += 1;
        playerYOffset = playerDirection.y;
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
                    facePlayer();
                }
                if (!isShooting && angleToPlayer <= shootAngle)
                {
                    StartCoroutine(shoot());
                }
                return true;
            }
        }
        return false;
    }
    public override IEnumerator shoot()
    {
        isShooting = true;

        GameObject bulletClone = Instantiate(bullet, shootPosition.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = playerDirection * bulletSpeed;

        if (shootPositionTwo != null)
        {
            GameObject bulletCloneTwo = Instantiate(bullet, shootPositionTwo.position, bullet.transform.rotation);
            bulletCloneTwo.GetComponent<Rigidbody>().velocity = playerDirection * bulletSpeed;

            GameObject bulletCloneThree = Instantiate(bullet, shootPositionThree.position, bullet.transform.rotation);
            bulletCloneThree.GetComponent<Rigidbody>().velocity = playerDirection * bulletSpeed;

            GameObject bulletCloneFour = Instantiate(bullet, shootPositionFour.position, bullet.transform.rotation);
            bulletCloneFour.GetComponent<Rigidbody>().velocity = playerDirection * bulletSpeed;
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
