using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mechHead : MonoBehaviour
{
    [SerializeField] Transform shootPositionMissile;
    [SerializeField] GameObject missile;
    [SerializeField] int missileSpeed;
    [SerializeField] float missileShootRate;
    [SerializeField] float missileYVelocity;
    [SerializeField] float missileRange;


    bool isMissileShoot;
    [SerializeField] GameObject parentBody;
    bool isOnHead;

    protected Vector3 playerDirection;
    protected float angleToPlayer;

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        if (parentBody.GetComponentInParent<BoxCollider>().enabled == false)
        {
            GetComponent<mechHead>().enabled = false;
        }
        else if (!isMissileShoot && isOnHead == true)
        {
            StartCoroutine(missileShoot());
        }
    }
    IEnumerator missileShoot()
    {
        isMissileShoot = true;
        createMissile();
        // anim.SetTrigger("ShootMissile");
        yield return new WaitForSeconds(missileShootRate);
        isMissileShoot = false;
    }
    public void createMissile()
    {
        playerDirection = (gameManager.instance.player.transform.position - transform.position).normalized;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);
        GameObject missileClone = Instantiate(missile, shootPositionMissile.position, missile.transform.rotation);
        Vector3 missileVector = (gameManager.instance.player.transform.position - shootPositionMissile.position).normalized;
        missileClone.GetComponent<Rigidbody>().velocity = (missileVector + new Vector3(0, missileYVelocity, 0)) * missileSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOnHead = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOnHead = false;
        }
    }
}
