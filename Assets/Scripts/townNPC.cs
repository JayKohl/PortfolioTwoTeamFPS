using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class townNPC : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    public NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform npcTransportPos;
    [SerializeField] bool hasQuestToGive;
    [SerializeField] bool hasSomethingToSay;
    [SerializeField] GameObject townPet;

    [Header("----- NPC Stats -----")]
    [SerializeField] Transform headPos;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int waitTime;
    [SerializeField] int roamDist;
    [SerializeField] int speedFast;


    public Transform orgPos;
    bool isPetDead;
    bool isGivenQuest;
    bool isPlayerInRange;
    float angleToPlayer;
    Vector3 playerDirection;
    Vector3 startingPos;
    bool destinationChosen;
    float stoppingDistOrig;
    float speedOrig;
    int followTime;
    int reFollow;

    [SerializeField] bool questOne;
    [SerializeField] bool questTwo;
    [SerializeField] bool isBathroom;


    // Start is called before the first frame update
    void Start()
    {
        isGivenQuest = false;
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        speedOrig = agent.speed;
        isPetDead = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (agent.isActiveAndEnabled)
        {
            //if (gameManager.instance.enemiesRemaining <= 0 && isGivenQuest && isDoorOpen == true)
            //{
            //    StartCoroutine(roam());
            //}
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
            if (isPlayerInRange && followTime <= 240)
            {
                if (!canSeePlayer())
                {
                    StartCoroutine(roam());
                }
                else
                {
                    followTime++;
                    //if (followTime >= 240)
                    //{
                    //    reFollow++;
                    //    StartCoroutine(roam());
                    //}
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
                reFollow++;
                StartCoroutine(roam());
                if (reFollow >= 960)
                {
                    reFollow = 0;
                    followTime = 0;
                }
            }
            //else
            //{
            //    StartCoroutine(roam());
            //}
        }
    }
    //IEnumerator setAgentOn()
    //{
    //    yield return new WaitForSeconds(1);
    //    //agent.enabled = true;
    //}
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

        //Debug.Log(angleToPlayer);
        //Debug.DrawRay(headPos.position, playerDirection);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                if (townPet.GetComponent<NavMeshAgent>().isActiveAndEnabled == false)
                {
                    isPetDead = true;
                }
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    facePlayer();
                }
                if (hasQuestToGive == true)
                {
                    if (questOne)
                    {

                        if (!isGivenQuest)
                        {
                            gameManager.instance.playerCamera.GetComponentInChildren<Objectivepoint>().SetWayPoint(gameManager.instance.playerCamera.GetComponentInChildren<Objectivepoint>().quest2);
                            orgPos = transform;
                            anim.SetTrigger("Talk");

                            gameManager.instance.displayNpcText("Our patrol was expected back in town three hours ago... I fear what may have happened. " +
                                "We know of an encampment just past Crab Wood Forest. Please see if you can find them, there are not many of us remaining.");
                            StartCoroutine(gameManager.instance.deleteTextNpc(12f));

                            gameManager.instance.infoText.text = "Find Missing Patrol";
                            gameManager.instance.infoTextBackground.SetActive(true);

                            hasQuestToGive = false;
                        }
                    }
                    else if (questTwo)
                    {
                        gameManager.instance.playerCamera.GetComponentInChildren<Objectivepoint>().SetWayPoint(gameManager.instance.playerCamera.GetComponentInChildren<Objectivepoint>().quest4);
                        orgPos = transform;
                        anim.SetTrigger("Talk");

                        gameManager.instance.displayNpcText("That complex over there seems to have risen over night. They ambushed us and trapped us " +
                            "in these cages. Maybe there is a terminal inside the complex. Please find a way to free us.");
                        StartCoroutine(gameManager.instance.deleteTextNpc(12f));

                        gameManager.instance.infoText.text = "Investigate The Enemy Complex";
                        gameManager.instance.infoTextBackground.SetActive(true);

                        hasQuestToGive = false;
                    }
                }
                else if (hasSomethingToSay)
                {
                    if (isPetDead == false)
                    {
                        anim.SetTrigger("Talk");
                        gameManager.instance.displayNpcText("Have you seen our town pet Leroy? Keep your distance he can be very cranky.");
                        StartCoroutine(gameManager.instance.deleteTextNpc(12f));
                    }
                    else
                    {
                        anim.SetTrigger("Talk");
                        gameManager.instance.displayNpcText("You monster, stay away from me. You killed Leroy, we will never find another town pet like him.");
                        StartCoroutine(gameManager.instance.deleteTextNpc(12f));
                    }
                }
                else if (isBathroom)
                {
                    anim.SetTrigger("Talk");
                    gameManager.instance.displayNpcText("Can I get you a towel sir?");
                    StartCoroutine(gameManager.instance.deleteTextNpc(12f));
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }
    IEnumerator setGoal(string text)
    {
        yield return new WaitForSeconds(8);
        gameManager.instance.infoText.text = text;
        gameManager.instance.infoTextBackground.SetActive(true);
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

    IEnumerator doorOne()
    {
        gameManager.instance.cam2.GetComponentInChildren<secondCamera>().openDoorOne();
        yield return new WaitForSecondsRealtime(3);

        gameManager.instance.cam2.GetComponentInChildren<secondCamera>().doorOne.SetActive(false);
        yield return new WaitForSecondsRealtime(3);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        transform.position = orgPos.position;
        transform.localRotation = orgPos.localRotation;
        gameManager.instance.playerScript.controller.enabled = true;
        gameManager.instance.playerCamera.SetActive(true);
        gameManager.instance.cam2.SetActive(false);

        gameManager.instance.playerScript.minimap.SetActive(true);
        gameManager.instance.playerHPBar.transform.parent.gameObject.SetActive(true);
        gameManager.instance.enemiesRemainingObject.SetActive(true);
        gameManager.instance.enemiesRemainingText.enabled = true;
        gameManager.instance.crosshair.SetActive(true);
        gameManager.instance.cam2.transform.GetChild(1).gameObject.SetActive(false);
        gameManager.instance.unPause();
        gameManager.instance.playerScript.canShoot = true;

        isGivenQuest = true;

    }

    IEnumerator doorTwo()
    {

        yield return new WaitForSecondsRealtime(1);
        gameManager.instance.playerScript.minimap.SetActive(false);
        gameManager.instance.playerHPBar.transform.parent.gameObject.SetActive(false);
        gameManager.instance.enemiesRemainingObject.SetActive(false);
        gameManager.instance.enemiesRemainingText.enabled = false;
        gameManager.instance.crosshair.SetActive(false);
        gameManager.instance.cam2.SetActive(true);
        gameManager.instance.playerCamera.SetActive(false);
        Time.timeScale = 0;

        gameManager.instance.cam2.GetComponentInChildren<secondCamera>().openDoorTwo();
        yield return new WaitForSecondsRealtime(3);
        gameManager.instance.cam2.GetComponentInChildren<secondCamera>().doorTwo.SetActive(false);
        yield return new WaitForSecondsRealtime(3);
        gameManager.instance.playerScript.controller.enabled = true;
        gameManager.instance.playerCamera.SetActive(true);
        gameManager.instance.cam2.SetActive(false);
        gameManager.instance.playerScript.minimap.SetActive(true);
        gameManager.instance.playerHPBar.transform.parent.gameObject.SetActive(true);
        gameManager.instance.enemiesRemainingObject.SetActive(true);
        gameManager.instance.enemiesRemainingText.enabled = true;
        gameManager.instance.crosshair.SetActive(true);
        gameManager.instance.unPause();
        gameManager.instance.playerScript.canShoot = true;

        gameManager.instance.playerCamera.GetComponentInChildren<Objectivepoint>().location = gameManager.instance.playerCamera.GetComponentInChildren<Objectivepoint>().objectiveOne;
        gameManager.instance.playerCamera.GetComponentInChildren<Objectivepoint>().locator.enabled = true;
        isGivenQuest = true;
    }

}
