using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class sentryGun : MonoBehaviour
{
    int viewAngle = 180;
    int shootAngle = 180;
    int enemyFaceSpeed = 60;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform headPos;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip startUpSound;
    [SerializeField] AudioClip audBasicAttack;
    float deleteTimer = 15;
    float shootRate = .1f;

    Vector3 enemyDirection;
    float angleToEnemy;

    bool alive;
    bool isEnemyInRange;
    bool isShooting;

    GameObject target;
    List<GameObject> enemiesNearby = new List<GameObject>();
    int next;

    // Make sure the NavMesh stopping distance is the same as the sphere collider trigger radius. 
    void Start()
    {
        next = 0;
        StartCoroutine(coolDownStart());
        muzzleFlash.GetComponent<ParticleSystem>().Stop();
        StartCoroutine(deathTimer());
    }

    //// Update is called once per frame
    void Update()
    {
        if (isEnemyInRange && alive)
        {
            float distance = Vector3.Distance(target.transform.position, transform.position);
            canSeeEnemy();
        }
    }
    IEnumerator coolDownStart()
    {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
        aud.PlayOneShot(startUpSound);
        yield return new WaitForSeconds(2);
        alive = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }
    IEnumerator deathTimer()
    {
        yield return new WaitForSeconds(deleteTimer);
        Destroy(gameObject);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBoss") || other.CompareTag("Turret"))
        {
            if (!enemiesNearby.Contains(other.gameObject))
            {
                enemiesNearby.Add(other.gameObject);
                target = enemiesNearby[next];
                isEnemyInRange = true;
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBoss") || other.CompareTag("Turret"))
        {
            if (!enemiesNearby.Contains(other.gameObject))
            {
                enemiesNearby.Add(other.gameObject);
                target = enemiesNearby[next];
                isEnemyInRange = true;
            }
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBoss") || other.CompareTag("Turret"))
        {
            if (!enemiesNearby.Contains(other.gameObject))
            {
                enemiesNearby.Add(other.gameObject);
                target = enemiesNearby[next];
                isEnemyInRange = true;
            }
        }
    }
    public bool canSeeEnemy()
    {
        enemyDirection = (target.transform.position - transform.position).normalized;
        angleToEnemy = Vector3.Angle(new Vector3(enemyDirection.x, 0, enemyDirection.z), transform.forward);

        Debug.DrawRay(headPos.position, enemyDirection);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, enemyDirection, out hit))
        {
            if ((hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("EnemyBoss") || hit.collider.CompareTag("Turret")) && angleToEnemy <= viewAngle)
            {
                if (isEnemyInRange)
                {
                    faceEnemy();
                }
                if (!isShooting && alive && angleToEnemy <= shootAngle)
                {
                    if (target.GetComponent<NavMeshAgent>().isActiveAndEnabled == true)
                    {
                        gameObject.GetComponent<NavMeshAgent>().enabled = true;
                        StartCoroutine(shoot());
                    }
                    else
                    {
                        if (next < enemiesNearby.Count)
                        {
                            next++;
                            if (next >= enemiesNearby.Count)
                            {
                                next = enemiesNearby.Count-1;                                
                            }
                            target = enemiesNearby[next];
                            isEnemyInRange = true;
                            //Debug.Log("Next: "+next+", Total:"+enemiesNearby.Count);
                        }
                        else
                        {
                            gameObject.GetComponent<NavMeshAgent>().enabled = false;
                            muzzleFlash.GetComponent<ParticleSystem>().Stop();
                            isEnemyInRange = false;
                        }
                    }
                }
                return true;
            }
        }
        return false;
    }

    public void faceEnemy()
    {
        Quaternion rotate = Quaternion.LookRotation(enemyDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.deltaTime * enemyFaceSpeed);
    }
    IEnumerator shoot()
    {
        muzzleFlash.GetComponent<ParticleSystem>().Play();
        isShooting = true;

        if (gameManager.instance.lvlbuttons.abilityDamageUp)
        {
            target.GetComponent<enemyAI>().takeDamage(6);
        }
        else
        {
            target.GetComponent<enemyAI>().takeDamage(3);
        }
        aud.PlayOneShot(audBasicAttack);
        Vector3 shootingVector = (target.transform.position - headPos.position).normalized;

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
        muzzleFlash.GetComponent<ParticleSystem>().Stop();
    }
}
