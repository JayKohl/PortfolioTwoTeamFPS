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
    [SerializeField] GameObject doorToBoss;

    [Header("----- NPC Stats -----")]
    [SerializeField] Transform headPos;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int waitTime;
    [SerializeField] int roamDist;
    [SerializeField] int speedFast;

    bool isDoorOpen;
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
        isDoorOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            Debug.Log(gameManager.instance.enemiesRemaining);
            if (gameManager.instance.enemiesRemaining <= 0 && isGivenQuest && isDoorOpen == false)
            {
                anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
                agent.SetDestination(moveToTerminal.position);
                Destroy(doorToBoss);
                isDoorOpen = true;
                gameManager.instance.displayNpcText("Hurry to the flight deck to secure your ship... I will hold off the reinforcements.");
                //exitArena();
            }
            else if (isDoorOpen == false)
            {

                anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
                if (isPlayerInRange)
                {
                    if (!canSeePlayer())
                    {
                        StartCoroutine(roam());
                    }
                    //else if (!isGivenQuest)
                    //{
                    //    isGivenQuest = true;
                    //    anim.SetTrigger("Talk");
                    //    gameManager.instance.displayNpcText("Chat Test");
                    //    StartCoroutine(gameManager.instance.deleteTextNpc(8));
                    //}
                }
                else if (agent.destination != gameManager.instance.player.transform.position)
                {
                    StartCoroutine(roam());
                }
            }
            //else
            //{
            //    StartCoroutine(roam());
            //}
        }
    }
    //protected IEnumerator exitArena()
    //{
    //    //agent.speed = speedFast;
    //    //anim.SetTrigger("Run");
    //    //agent.SetDestination(moveToTerminal.transform.position);
    //    yield return new WaitForSeconds(2);
    //    Destroy(doorToBoss);
    //    isDoorOpen = true;
    //    gameManager.instance.displayNpcText("Hurry to the flight to secure your ship... I will hold off the reinforcements.");
    //    //StartCoroutine(roam());
    //}
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
                    gameManager.instance.displayNpcText("Listen, we do not have much time. They have brought you here to be a combatant in the arena. " +
                                                        "If by chance you can survive I will help you escape. Now go away before anyone notices us talking.");
                    StartCoroutine(gameManager.instance.deleteTextNpc(8));
                }
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
