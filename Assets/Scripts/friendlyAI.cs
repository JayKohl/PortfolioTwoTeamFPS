using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class friendlyAI : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    public NavMeshAgent agent;
    [SerializeField] Animator anim;

    [Header("----- NPC Stats -----")]
    [SerializeField] Transform headPos;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int waitTime;
    [SerializeField] int roamDist;
    [SerializeField] int speedFast;

    bool isPlayerInRange;
    bool isTalking;
    float angleToPlayer;
    Vector3 playerDirection;
    Vector3 startingPos;
    Vector3 moveToTerminal;
    bool destinationChosen;
    float stoppingDistOrig;
    float speedOrig;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        speedOrig = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
            if (true)
            {

            }
        }
    }
    private bool canSeePlayer()
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
                agent.SetDestination(gameManager.instance.player.transform.position);
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    facePlayer();
                }
                if (!isTalking && angleToPlayer <= viewAngle)
                {
                    // call the methods to talk to the player.
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }
    private void facePlayer()
    {
        playerDirection.y = 0;
        Quaternion rotate = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.deltaTime * playerFaceSpeed);
    }
}
