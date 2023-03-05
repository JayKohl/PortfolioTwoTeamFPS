using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyThirdBoss : enemyAI
{
    [SerializeField] GameObject topPiece;
    bool isFlip;
    bool isSpawnEvent;
    int hitPointsOrig;
    int fullFlip;
    // Start is called before the first frame update
    void Start()
    {
        hitPointsOrig = hitPoints;
        agentStop();
        isFlip = false;
        Vector3 StartingPos = topPiece.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFlip && !isSpawnEvent)
        {
            canSeePlayer();
            topPiece.transform.Rotate(0f, 1f, 0f, Space.Self);
        }
        else if (isFlip)
        {
            topPiece.transform.Rotate(.5f, 0f, 0f);
            fullFlip++;
            if (fullFlip >= 360)
            {
                isFlip = false;
                fullFlip = 0;
            }
        }
    }
    //private void flipObject()
    //{
    //    isFlip = true;
    //    topPiece.transform.Rotate(180f, 0f, 0f, Space.Self);
    //}
    public override void takeDamage(int dmg)
    {
        hitPoints -= dmg;
        if (hitPoints <= 0)
        {
            GetComponent<Collider>().enabled = false;
            //GetComponentInChildren<Canvas>().enabled = false;
            //aud.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)], audDeathVol);
            agent.enabled = false;
            Destroy(gameObject);
        }
        else
        {
            //anim.SetTrigger("Damage");
            if (dmg > 0)
            {
                aud.PlayOneShot(audTakeDamage[Random.Range(0, audTakeDamage.Length)], audTakeDamageVol);
            }
            StartCoroutine(flashDamage());
        }
        if (hitPoints <= hitPointsOrig / 2)
        {
            isSpawnEvent = true;
            isFlip = true;
        }
    }
    protected override bool canSeePlayer()
    {
        playerDirection = (gameManager.instance.player.transform.position - headPos.position).normalized;
        // playerDirection.y += 1;
        //playerYOffset = playerDirection.y;
        //playerDirection = gameManager.instance.player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);

        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDirection);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player"))// && angleToPlayer <= viewAngle)
            {
                //agent.stoppingDistance = stoppingDistOrig;
                //agent.speed = speedChase;
                //agent.SetDestination(gameManager.instance.player.transform.position);
                //if (agent.remainingDistance < agent.stoppingDistance)
                //{
                //    //facePlayer();
                //}
                if (!isShooting) //&& angleToPlayer <= shootAngle)
                {
                    StartCoroutine(shoot());
                }
                return true;
            }
        }
        //agent.stoppingDistance = 0;
        return false;
    }
    protected override IEnumerator shoot()
    {
        isShooting = true;
        createBullet();
        //anim.SetTrigger("Shoot");
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
    public override void createBullet()
    {
        if (!blind)
        {
            GameObject bulletClone = Instantiate(bullet, shootPosition.position, bullet.transform.rotation);
            bulletClone.GetComponent<Rigidbody>().velocity = playerDirection * bulletSpeed;
        }
        else
        {
            GameObject bulletClone = Instantiate(bullet, shootPosition.position, bullet.transform.rotation);
            bulletClone.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(0, 0.5f), Random.Range(0, 0.5f), Random.Range(0, 0.5f)) * bulletSpeed;
        }
        //aud.PlayOneShot(audBasicAttack[Random.Range(0, audBasicAttack.Length)], audBasicAttackVol);
    }
}
