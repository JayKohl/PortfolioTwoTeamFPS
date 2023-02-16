using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// getting access to the nav mesh.
using UnityEngine.AI;

// I added this class to be abstract for use with childeren.
public abstract class enemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    // For flashing the material red "visual feedback"
    [SerializeField] Renderer model;
    // This is to attach the enemy to the nav mesh. 
    public NavMeshAgent agent;
    //[SerializeField] GameObject fuelCap;
    [SerializeField] public Animator anim;

    [Header("----- Enemy Stats -----")]
    [SerializeField] public float playerYOffset;
    [SerializeField] public Transform headPos;
    public int hitPoints;
    [SerializeField] public int playerFaceSpeed;
    [SerializeField] public int viewAngle;
    [SerializeField] public int shootAngle;
    [SerializeField] int waitTime;
    [SerializeField] int roamDist;

    [Header("----- Gun -----")]
    [SerializeField] public Transform shootPosition;
    [SerializeField] public GameObject bullet;
    [SerializeField] public int bulletSpeed;
    [SerializeField] public float shootRate;

    public Vector3 playerDirection;
    public bool isPlayerInRange;
    public bool isShooting;
    public float angleToPlayer;

    public Vector3 startingPos;
    bool destinationChosen;
    public float stoppingDistOrig;

    // Start is called before the first frame update
    //void Start()
    //{
    //    if (gameObject.CompareTag("EnemyBoss"))
    //    {
    //        gameManager.instance.updateGameGoal(+1);
    //    }

    //    startingPos = transform.position;
    //    stoppingDistOrig = agent.stoppingDistance;

    //    roam();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    anim.SetFloat("Speed", agent.velocity.normalized.magnitude);

    //    if (isPlayerInRange)
    //    {
    //        if (!canSeePlayer() && agent.remainingDistance < 0.1f)
    //        {
    //            roam();
    //        }
    //    }
    //    else if (agent.remainingDistance < 0.1f && agent.destination != gameManager.instance.player.transform.position)
    //    {
    //        roam();
    //    }
    //}

    public IEnumerator roam()
    {
        if (!destinationChosen && agent.remainingDistance < 0.1f)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(waitTime);
            destinationChosen = false;

            Vector3 randDir = Random.insideUnitSphere * roamDist;
            randDir += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randDir, out hit, roamDist, NavMesh.AllAreas);
            agent.SetDestination(hit.position);
        }
    }
    //public void roam()
    //{
    //    agent.stoppingDistance = 0;

    //    Vector3 randDir = Random.insideUnitSphere * roamDist;
    //    randDir += startingPos;

    //    NavMeshHit hit;
    //    NavMesh.SamplePosition(randDir, out hit, 1, 1);
    //    NavMeshPath path = new NavMeshPath();

    //    if (hit.position != null)
    //    {
    //        agent.CalculatePath(hit.position, path);
    //    }
    //    agent.SetPath(path);
    //}
    public virtual bool canSeePlayer()
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
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);
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
        agent.stoppingDistance = 0;
        return false;
    }

    public virtual void takeDamage(int dmg)
    {
        hitPoints -= dmg;
        agent.SetDestination(gameManager.instance.player.transform.position);
        StartCoroutine(flashDamage());
        if (hitPoints <= 0)
        {
            // this checks to see if the enemy killed is a boss. if true it drops a object.
            //if (gameObject.CompareTag("EnemyBoss"))
            //{
            //    //GameObject fuel = Instantiate(fuelCap, gameObject.transform.position, fuelCap.transform.rotation);
            //    //gameManager.instance.updateGameGoal(-1);
            //}
            Destroy(gameObject);
        }
    }
    public IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        model.material.color = Color.white;
    }
    public virtual void facePlayer()
    {
        playerDirection.y = 0;
        Quaternion rotate = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.deltaTime * playerFaceSpeed);
    }
    public virtual IEnumerator shoot()
    {
        isShooting = true;
        GameObject bulletClone = Instantiate(bullet, shootPosition.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = playerDirection * bulletSpeed;

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
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
