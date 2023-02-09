using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// getting access to the nav mesh.
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    // For flashing the material red "visual feedback"
    [SerializeField] Renderer model;
    // This is to attach the enemy to the nav mesh. 
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject fuelCap;

    [Header("----- Enemy Stats -----")]
    [SerializeField] Transform headPos;
    [SerializeField] int hitPoints;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int viewAngle;

    [Header("----- Gun -----")]
    [SerializeField] Transform shootPosition;
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletSpeed;
    [SerializeField] float shootRate;

    Vector3 playerDirection;
    bool isPlayerInRange;
    bool isShooting;
    float angleToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.CompareTag("EnemyBoss"))
        {
            gameManager.instance.updateGameGoal(+1);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (isPlayerInRange && canSeePlayer())
        {
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                facePlayer();
            }
            if (!isShooting)
            {
                StartCoroutine(shoot());
            }
        }
    }

    bool canSeePlayer()
    {
        playerDirection = gameManager.instance.player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);

        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDirection);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
                return true;
            }
        }
        return false;
    }

    public void takeDamage(int dmg)
    {
        hitPoints -= dmg;
        StartCoroutine(flashDamage());
        if (hitPoints <= 0)
        {
            // this checks to see if the enemy killed is a boss. if true it drops a object.
            if (gameObject.CompareTag("EnemyBoss"))
            {
                GameObject fuel = Instantiate(fuelCap, gameObject.transform.position, fuelCap.transform.rotation);
            }
            Destroy(gameObject);
        }
    }
    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        model.material.color = Color.white;
    }
    void facePlayer()
    {
        Quaternion rotate = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.deltaTime * playerFaceSpeed);
    }
    IEnumerator shoot()
    {
        isShooting = true;
        GameObject bulletClone = Instantiate(bullet, shootPosition.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

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
        }
    }
}
