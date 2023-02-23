using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// getting access to the nav mesh.
using UnityEngine.AI;

// I added this class to be abstract for use with childeren.
public abstract class enemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    public NavMeshAgent agent;
    [SerializeField] protected Animator anim;
    [SerializeField] AudioSource aud;

    [Header("----- Enemy Stats -----")]
    [SerializeField] protected float playerYOffset;
    [SerializeField] protected Transform headPos;
    [SerializeField] public int hitPoints;
    [SerializeField] protected int playerFaceSpeed;
    [SerializeField] protected int speedChase;
    [SerializeField] protected int viewAngle;
    [SerializeField] protected int shootAngle;
    [SerializeField] int waitTime;
    [SerializeField] int roamDist;

    [Header("----- Gun -----")]
    [SerializeField] protected Transform shootPosition;
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected int bulletSpeed;
    [SerializeField] protected float shootRate;

    [Header("----- Melee -----")]
    [SerializeField] protected Collider meleeCollider;
    [SerializeField] protected float meleeRate;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audTakeDamage;
    [Range(0, 1)] [SerializeField] float audTakeDamageVol;
    [SerializeField] AudioClip[] audDeath;
    [Range(0, 1)] [SerializeField] float audDeathVol;

    protected Vector3 playerDirection;
    public bool isPlayerInRange;
    protected bool isShooting;
    protected bool isMelee;
    protected float angleToPlayer;
    protected float speedOrig;
    protected Vector3 startingPos;
    bool destinationChosen;
    protected float stoppingDistOrig;


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
    protected virtual bool canSeePlayer()
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
                agent.speed = speedChase;
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
        if (hitPoints <= 0)
        {
            GetComponent<Collider>().enabled = false;
            GetComponentInChildren<Canvas>().enabled = false;
            aud.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)], audDeathVol);
            anim.SetBool("Dead", true);
            agent.enabled = false;
            //Destroy(gameObject); Create a IEnumerator for destroyObject
        }
        else
        {
            aud.PlayOneShot(audDeath[Random.Range(0, audTakeDamage.Length)], audTakeDamageVol);
            anim.SetTrigger("Damage");
            // melee add a function for turning off the weapon collider.
            agent.SetDestination(gameManager.instance.player.transform.position);
            StartCoroutine(flashDamage());
        }
    }
    protected IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        model.material.color = Color.white;
    }
    protected virtual void facePlayer()
    {
        playerDirection.y = 0;
        Quaternion rotate = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.deltaTime * playerFaceSpeed);
    }
    protected virtual IEnumerator melee()
    {
        isMelee = true;
        anim.SetTrigger("Melee");
        yield return new WaitForSeconds(meleeRate);
        isMelee = false;
    }
    protected virtual IEnumerator shoot()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
    public virtual void createBullet()
    {
        GameObject bulletClone = Instantiate(bullet, shootPosition.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = playerDirection * bulletSpeed;
    }
    public void meleeColliderOn()
    {
        meleeCollider.enabled = true;
    }
    public void meleeColliderOff()
    {
        meleeCollider.enabled = false;
    }
    public void agentStop()
    {
        agent.enabled = false;
    }
    public void agentStart()
    {
        agent.enabled = true;
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
