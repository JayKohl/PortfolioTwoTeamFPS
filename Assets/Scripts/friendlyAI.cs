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
    [SerializeField] Transform moveToTerminal;

    [Header("----- NPC Stats -----")]
    [SerializeField] Transform headPos;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int waitTime;
    [SerializeField] int roamDist;
    [SerializeField] int speedFast;

    bool isGivenQuest;
    bool isPlayerInRange;
    bool isTalking;
    float angleToPlayer;
    Vector3 playerDirection;
    Vector3 startingPos;
    bool destinationChosen;
    float stoppingDistOrig;
    float speedOrig;

    // Start is called before the first frame update
    void Start()
    {
        isGivenQuest = false;
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
            if (isPlayerInRange)
            {
                if (!canSeePlayer())
                {
                    StartCoroutine(roam());
                }
                else if (!isGivenQuest)
                {
                    isGivenQuest = true;
                    anim.SetTrigger("Talk");
                    gameManager.instance.displayNpcText("Chat Test");
                    StartCoroutine(gameManager.instance.deleteTextNpc(8));
                }
            }
            else if (agent.destination != gameManager.instance.player.transform.position)
            {
                StartCoroutine(roam());
            }
        }
    }
    protected IEnumerator roam()
    {
        if (!destinationChosen && agent.remainingDistance < 0.1f)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            agent.speed = speedOrig;
            yield return new WaitForSeconds(waitTime);
            destinationChosen = false;

            if (agent.isActiveAndEnabled)
            {
                Vector3 randDir = Random.insideUnitSphere * roamDist;
                randDir += startingPos;
                NavMeshHit hit;
                NavMesh.SamplePosition(randDir, out hit, roamDist, NavMesh.AllAreas);
                agent.SetDestination(hit.position);
            }
        }
    }
    protected bool canSeePlayer()
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
                if (!isGivenQuest)
                {
                    isGivenQuest = true;
                    anim.SetTrigger("Talk");
                    gameManager.instance.displayNpcText("Chat Test");
                    StartCoroutine(gameManager.instance.deleteTextNpc(8));
                }
                //if (!isTalking && angleToPlayer <= viewAngle)
                //{
                //    facePlayer();
                //    // call the methods to talk to the player.
                //}
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }
    protected void facePlayer()
    {
        playerDirection.y = 0;
        Quaternion rotate = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.deltaTime * playerFaceSpeed);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            agent.stoppingDistance = 0;
        }
    }
}
