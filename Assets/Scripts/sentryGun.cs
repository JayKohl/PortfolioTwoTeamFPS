using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class sentryGun : MonoBehaviour
{
    //int viewAngle = 90;
    int shootAngle = 90;
    int enemyFaceSpeed = 60;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform headPos;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip startUpSound;
    [SerializeField] AudioClip audBasicAttack;
    [SerializeField] float audBasicAttackVol;
    float deleteTimer = 15;
    //int bulletSpeed = 25;
    float shootRate = .3f;

    Vector3 enemyDirection;
    float angleToEnemy;

    bool alive;
    bool isEnemyInRange;
    bool isShooting;

    GameObject target;

    // Make sure the NavMesh stopping distance is the same as the sphere collider trigger radius. 
    void Awake()
    {
        StartCoroutine(coolDownStart());
        muzzleFlash.GetComponent<ParticleSystem>().Stop();
        StartCoroutine(deathTimer());
    }

    //// Update is called once per frame
    void Update()
    {
        if (isEnemyInRange && alive)
        {
            canSeeEnemy();
        }
    }
    IEnumerator coolDownStart()
    {
        aud.PlayOneShot(startUpSound, audBasicAttackVol);
        yield return new WaitForSeconds(2);
        alive = true;
    }
    IEnumerator deathTimer()
    {
        yield return new WaitForSeconds(deleteTimer);
        Destroy(gameObject);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBoss"))
        {
            target = other.gameObject;
            isEnemyInRange = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBoss"))
        {
            target = other.gameObject;
            isEnemyInRange = true;
        }
    }
    public bool canSeeEnemy()
    {
        enemyDirection = (target.transform.position - headPos.position).normalized;
        angleToEnemy = Vector3.Angle(new Vector3(enemyDirection.x, 0, enemyDirection.z), transform.forward);

        Debug.DrawRay(headPos.position, enemyDirection);
        if (isEnemyInRange)
        {
            faceEnemy();
        }
        if (!isShooting && angleToEnemy <= shootAngle)
        {
            if (alive && target.GetComponent<NavMeshAgent>().isActiveAndEnabled == true)
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
                StartCoroutine(shoot());
            }
            else
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
                muzzleFlash.GetComponent<ParticleSystem>().Stop();
            }
        }

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, enemyDirection, out hit))
        //{
        //    if (hit.collider.CompareTag("Enemy") && angleToEnemy <= viewAngle)
        //    {
        //        if (isEnemyInRange)
        //        {
        //            faceEnemy();
        //        }
        //        if (!isShooting && angleToEnemy <= shootAngle)
        //        {
        //            StartCoroutine(shoot());
        //        }
        //        return true;
        //    }
        //}
        return false;
    }

    public void faceEnemy()
    {
        //if (!isInCoolDown)
        {
            Quaternion rotate = Quaternion.LookRotation(enemyDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.deltaTime * enemyFaceSpeed);
        }
    }
    IEnumerator shoot()
    {
        //if (!isInCoolDown)
        {
            muzzleFlash.GetComponent<ParticleSystem>().Play();
            isShooting = true;

            if (gameManager.instance.lvlMenu.GetComponent<LVLButtons>().abilityDamageUp)
            {
                target.GetComponent<enemyAI>().takeDamage(4);
            }
            else
            {
                target.GetComponent<enemyAI>().takeDamage(1);
            }
            //GameObject bulletClone = Instantiate(bullet, headPos.position, bullet.transform.rotation);
            aud.PlayOneShot(audBasicAttack, audBasicAttackVol);
            Vector3 shootingVector = (target.transform.position - headPos.position).normalized;
            //bulletClone.GetComponent<Rigidbody>().velocity = shootingVector * bulletSpeed;

            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }
    //public override void takeDamage(int dmg)
    //{
    //    if (gameManager.instance.playerScript.fireOn && !setOnFire)
    //    {
    //        setOnFire = true;
    //        StartCoroutine(onFire());
    //    }
    //    else if (gameManager.instance.playerScript.iceOn && !chilled)
    //    {
    //        chilled = true;
    //        chilledOnce = true;
    //        StartCoroutine(iced());
    //    }
    //    if (dmg > 0)
    //    {
    //        hitPoints -= dmg;
    //    }
    //    if (chilled && chilledOnce)
    //    {
    //        chilledOnce = false;
    //        shootRate = shootRate * 8;
    //    }
    //    StartCoroutine(flashDamage());
    //    if (hitPoints <= 0)
    //    {
    //        if (SceneManager.GetActiveScene().name == "LvlThreeTheWorld")
    //        {
    //            gameManager.instance.updateGameGoalLvl3(-1);
    //        }
    //        GetComponent<Collider>().enabled = false;
    //        GetComponentInChildren<Canvas>().enabled = false;
    //        if (!setOnFire)
    //        {
    //            aud.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)], audDeathVol);
    //        }
    //        agent.enabled = false;
    //        alive = false;
    //        StartCoroutine(die());
    //    }
    //    else
    //    {
    //        if (dmg > 0)
    //        {
    //            aud.PlayOneShot(audTakeDamage[Random.Range(0, audTakeDamage.Length)], audTakeDamageVol);
    //        }
    //    }
    //}
    //IEnumerator die()
    //{
    //    explosion.SetActive(true);
    //    plasmaExplosion.SetActive(true);
    //    deathFlames.SetActive(true);
    //    yield return new WaitForSeconds(2);
    //    explosion.SetActive(false);
    //    plasmaExplosion.SetActive(false);
    //}
    //public IEnumerator hacking(GameObject target)
    //{
    //    if (target == gameObject)
    //    {
    //        isInCoolDown = true;
    //        yield return new WaitForSeconds(10);
    //        isInCoolDown = false;
    //    }
    //}
    //IEnumerator onFire()
    //{
    //    yield return new WaitForSeconds(.5f);
    //    takeDamage(1);
    //    yield return new WaitForSeconds(1);
    //    takeDamage(1);
    //    yield return new WaitForSeconds(1);
    //    takeDamage(1);
    //    setOnFire = false;
    //}
    //IEnumerator iced()
    //{
    //    yield return new WaitForSeconds(.5f);
    //    iceEffect.SetActive(true);
    //    takeDamage(1);
    //    yield return new WaitForSeconds(1);
    //    takeDamage(1);
    //    yield return new WaitForSeconds(6);
    //    iceEffect.SetActive(false);
    //    chilled = false;
    //}
}
