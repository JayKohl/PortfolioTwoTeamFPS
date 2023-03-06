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
    [SerializeField] public GameObject doorToBoss;
    [SerializeField] Transform playerTransportPos;
    [SerializeField] Transform npcTransportPos;
    
    [SerializeField] public GameObject doorOutOfCell;

    [Header("----- NPC Stats -----")]
    [SerializeField] Transform headPos;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int waitTime;
    [SerializeField] int roamDist;
    [SerializeField] int speedFast;
   

    public Transform orgPos;
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
        
        gameManager.instance.cam2.SetActive(false);
        gameManager.instance.cam2.transform.GetChild(1).gameObject.SetActive(false);
        isGivenQuest = false;
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        speedOrig = agent.speed;
        isDoorOpen = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X) && isTalking)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameManager.instance.cam2.transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(doorOne());
        }

        if (agent.isActiveAndEnabled)
        {
            Debug.Log(gameManager.instance.enemiesRemaining);
            if (gameManager.instance.enemiesRemaining <= 0 && isGivenQuest && isDoorOpen == false)
            {
                gameManager.instance.playerScript.canShoot = false;
                gameManager.instance.cam2.transform.GetChild(1).gameObject.SetActive(true);
                //anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
                agent.enabled = false;
                transform.position = moveToTerminal.transform.position;
                //StartCoroutine(setAgentOn());
                anim.SetTrigger("Idle");
                //agent.enabled = false;
                //agent.enabled = false;
                //agent.enabled = true;
                StartCoroutine(doorTwo());              
                isDoorOpen = true;
                gameManager.instance.displayNpcText("Hurry to the flight deck to secure your ship... I will hold off the reinforcements.");
                StartCoroutine(setGoal("Get to the flight deck"));
                StartCoroutine(gameManager.instance.deleteTextNpc(8));
                //exitArena();
            }
            //if (gameManager.instance.enemiesRemaining <= 0 && isGivenQuest && isDoorOpen == true)
            //{
            //    StartCoroutine(roam());
            //}
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
                    isTalking = true;
                    orgPos = transform;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    transform.position = npcTransportPos.position;
                    transform.localRotation = npcTransportPos.localRotation;
                    gameManager.instance.playerScript.controller.enabled = false;
                    gameManager.instance.cam2.SetActive(true);
                    
                    gameManager.instance.playerCamera.SetActive(false);

                    anim.SetTrigger("Talk");
                    gameManager.instance.displayNpcText("Listen, we do not have much time. They have brought you here to be a combatant in the arena. \n\n" +
                                                        "If by chance you can survive I will help you escape. Now go away before anyone notices us talking.");


                   
                    gameManager.instance.playerScript.minimap.SetActive(false);                   
                    gameManager.instance.playerHPBar.transform.parent.gameObject.SetActive(false);
                    gameManager.instance.enemiesRemainingObject.SetActive(false);
                    gameManager.instance.enemiesRemainingText.enabled = false;
                    gameManager.instance.crosshair.SetActive(false);
                    gameManager.instance.playerScript.canShoot = false;
                    gameManager.instance.pause();
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

        isGivenQuest = true;
        isTalking = false;
    }
    
}
