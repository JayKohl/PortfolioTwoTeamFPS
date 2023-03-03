using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyMechAI : enemyAI
{
    [SerializeField] Transform shootPositionTwo;
    [SerializeField] Transform shootPositionThree;
    [SerializeField] Transform shootPositionFour;

    [SerializeField] Transform shootPositionMissile;
    [SerializeField] GameObject missile;
    [SerializeField] int missileSpeed;
    [SerializeField] float missileShootRate;
    [SerializeField] float missileYVelocity;
    [SerializeField] float missileRange;

    [Header("----- Effects -----")]
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject plasmaExplosion;
    [SerializeField] GameObject deathFlames;

    [Header("----- Audio Cont -----")]
    [SerializeField] protected AudioClip[] audMissile;
    [Range(0, 1)] [SerializeField] protected float audMissileVol;

    bool isMissileShoot;
    bool isShootingTwo;
    int hitPointsOrig;

    void Start()
    {
        hitPointsOrig = hitPoints;
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        speedOrig = agent.speed;
    }

    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
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
}
