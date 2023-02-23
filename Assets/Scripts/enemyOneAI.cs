using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemyOneAI : enemyAI
{
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        speedOrig = agent.speed;

        if (SceneManager.GetActiveScene().name == "LvlTwoTheArena")
        {
            gameManager.instance.updateGameGoalLvl2(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);

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
    public override void takeDamage(int dmg)
    {
        hitPoints -= dmg;
        if (hitPoints <= 0)
        {

            GetComponent<Collider>().enabled = false;
            GetComponentInChildren<Canvas>().enabled = false;
            aud.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)], audDeathVol);
            anim.SetBool("Dead", true);
            agent.enabled = false;
            if (SceneManager.GetActiveScene().name == "LvlTwoTheArena")
            {
                gameManager.instance.updateGameGoalLvl2(-1);
            }
            //Destroy(gameObject); Create a IEnumerator for destroyObject
        }
        else
        {
            anim.SetTrigger("Damage");
            aud.PlayOneShot(audTakeDamage[Random.Range(0, audTakeDamage.Length)], audTakeDamageVol);
            // melee add a function for turning off the weapon collider.
            agent.SetDestination(gameManager.instance.player.transform.position);
            StartCoroutine(flashDamage());
        }
    }
}
