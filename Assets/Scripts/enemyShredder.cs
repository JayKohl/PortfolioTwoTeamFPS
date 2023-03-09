using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemyShredder : enemyAI
{
    [SerializeField] GameObject fireEffect;
    bool setOnFire;
    [SerializeField] GameObject iceEffect;
    [SerializeField] float distanceToHit;
    bool chilled;
    bool chilledOnce;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        speedOrig = agent.speed;
        speedChaseOrig = speedChase;
    }

    // Update is called once per frame
    void Update()
    {        
        if (agent.isActiveAndEnabled)
        {
            if (!chilled)
            {
                meleeRate = meleeRateOrig;
                agent.speed = speedOrig;
                speedChase = speedChaseOrig;
            }
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
            if (isPlayerInRange)
            {
                if (!canSeePlayer())
                {
                    agent.destination = gameManager.instance.player.transform.position;
                }
            }
            else if (agent.destination != gameManager.instance.player.transform.position)
            {
                StartCoroutine(roam());
            }
        }
    }
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
                agent.stoppingDistance = stoppingDistOrig;
                agent.speed = speedChase;
                agent.SetDestination(gameManager.instance.player.transform.position);
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    facePlayer();
                }
                if (!isMelee && angleToPlayer <= shootAngle && distanceToEnemy <= distanceToHit)
                {
                    StartCoroutine(melee());
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }
    public override void takeDamage(int dmg)
    {
        if (gameManager.instance.playerScript.fireOn && !setOnFire)
        {
            setOnFire = true;
            StartCoroutine(onFire());
        }
        else if (gameManager.instance.playerScript.iceOn && !chilled)
        {
            chilled = true;
            chilledOnce = true;
            StartCoroutine(iced());
        }
        if (dmg > 0)
        {
            hitPoints -= dmg;
        }
        if (chilled && chilledOnce)
        {
            chilledOnce = false;
            agent.speed = speedOrig / 4;
            meleeRate = meleeRate * 8;
            speedChase = speedChase / 4;
        }
        if (hitPoints <= 0)
        {
            gameManager.instance.lvlscript.GainExperiance(xp);
            if (setOnFire)
            {
                model.material.color = Color.black;
            }
            if (chilled)
            {
                model.material.color = new Color(0, 0.5509f, 1);
                agent.enabled = false;
                GetComponent<Collider>().enabled = false;
                GetComponentInChildren<Canvas>().enabled = false;
                anim.enabled = false;
            }
            else
            {
                if (SceneManager.GetActiveScene().name == "LvlThreeTheWorld")
                {
                    gameManager.instance.updateGameGoalLvl3(-1);
                }
                GetComponent<Collider>().enabled = false;
                GetComponentInChildren<Canvas>().enabled = false;
                aud.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)], audDeathVol);
                anim.SetBool("Dead", true);
                agent.enabled = false;
            }
            //Destroy(gameObject); Create a IEnumerator for destroyObject
        }
        else
        {
            anim.SetTrigger("Damage");
            if (dmg > 0)
            {
                aud.PlayOneShot(audTakeDamage[Random.Range(0, audTakeDamage.Length)], audTakeDamageVol);
            }
            // melee add a function for turning off the weapon collider.
            agent.SetDestination(gameManager.instance.player.transform.position);
            StartCoroutine(flashDamage());
        }
    }
    IEnumerator onFire()
    {
        yield return new WaitForSeconds(.5f);
        fireEffect.SetActive(true);
        takeDamage(1);
        yield return new WaitForSeconds(1);
        takeDamage(1);
        yield return new WaitForSeconds(1);
        takeDamage(1);
        yield return new WaitForSeconds(2);
        if (gameManager.instance.lvlMenu.GetComponent<LVLButtons>().abilityDamageUp)
        {
            takeDamage(3);
        }
            fireEffect.SetActive(false);
        setOnFire = false;
    }
    IEnumerator iced()
    {
        yield return new WaitForSeconds(.5f);
        iceEffect.SetActive(true);
        takeDamage(1);
        yield return new WaitForSeconds(1);
        takeDamage(1);
        yield return new WaitForSeconds(6);
        takeDamage(2);
        iceEffect.SetActive(false);
        chilled = false;
    }
}
