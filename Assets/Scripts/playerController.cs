using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(5, 10)] [SerializeField] public int HP;
    [Range(1, 50)] [SerializeField] int playerSpeed;
    [Range(1, 3)] [SerializeField] int jumpTimes;
    [Range(10, 25)] [SerializeField] int jumpSpeed;
    [Range(15, 45)] [SerializeField] int gravity;
    [SerializeField] int runSpeed;

    [Header("----- Gun Stats -----")]
    [SerializeField] List<weaponStats> weaponList = new List<weaponStats>();
    [SerializeField] GameObject weaponModel;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int shootDamage;

    // Deactivated temp
    // [SerializeField] int bulletSpeed;
    [SerializeField] Transform shootPositionPlayer;
    // Deactivated temp
    // [SerializeField] GameObject bullet;
    [SerializeField] GameObject gunFlash;    

    int jumpsCurrent;
    Vector3 move;
    Vector3 playerVelocity;
    bool isShooting;
    bool isRunning;
    public int hpOriginal;
    public int speedOriginal;

    // Start is called before the first frame update
    void Start()
    {
        hpOriginal = HP;
        updatePlayerHPBar();
        speedOriginal = playerSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        if (!isShooting && Input.GetButton("Shoot"))
            StartCoroutine(shoot());
    }

    void movement()
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
            jumpsCurrent = 0;
        }
        move = (transform.right * Input.GetAxis("Horizontal") +
               (transform.forward * Input.GetAxis("Vertical")));

        controller.Move(move * Time.deltaTime * playerSpeed);

        if (Input.GetButtonDown("Jump") && jumpsCurrent < jumpTimes)
        {
            jumpsCurrent++;
            playerVelocity.y = jumpSpeed;
        }
        if(Input.GetButton("Run") && !isRunning)
        {
            isRunning = true;
            playerSpeed = runSpeed;
        }
        if(isRunning && Input.GetButtonUp("Run"))
        {
            isRunning = false;
            playerSpeed = speedOriginal;
        }
        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    IEnumerator shoot()
    {
        isShooting = true;
        StartCoroutine(gunShootFlash());
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            Debug.Log(hit.collider.name);

            // Deactivated temp
            // GameObject bulletClone = Instantiate(bullet, shootPositionPlayer.position, bullet.transform.rotation);
            // bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
            

            if (hit.collider.GetComponent<IDamage>() != null)
            {
                hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        updatePlayerHPBar();
        StartCoroutine(flashDamage());

        if (HP <= 0)
            gameManager.instance.playerDead();
    }

    IEnumerator gunShootFlash()
    {
        GameObject flash = Instantiate(gunFlash, shootPositionPlayer.position, gunFlash.transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Destroy(flash);
    }
    IEnumerator flashDamage()
    {
        gameManager.instance.playerDamageFlashScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDamageFlashScreen.SetActive(false);
    }

    public void giveHP(int amount)
    {
        HP += amount;
        if (HP > hpOriginal)
            HP = hpOriginal;
        updatePlayerHPBar();
    }

    public void updatePlayerHPBar()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / (float)hpOriginal;
    }
    public void weaponPickup(weaponStats gunstat)
    {
        weaponList.Add(gunstat);

        shootRate = gunstat.shootRate;
        shootDist = gunstat.shootDist;
        shootDamage = gunstat.shootDamage;

        weaponModel.GetComponent<MeshFilter>().sharedMesh = gunstat.weaponModel.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = gunstat.weaponModel.GetComponent<MeshRenderer>().sharedMaterial;
    }
}
