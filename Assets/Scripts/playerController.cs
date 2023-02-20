using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] public CharacterController controller;
    [SerializeField] Animator playeranim;

    [Header("----- Player Stats -----")]
    [Range(5, 10)] [SerializeField] public int HP;
    [Range(1, 50)] [SerializeField] int playerSpeed;
    [Range(1, 3)] [SerializeField] int jumpTimes;
    [Range(10, 25)] [SerializeField] int jumpSpeed;
    [Range(15, 45)] [SerializeField] int gravity;
    [SerializeField] int runSpeed;
    [SerializeField] float pushbackResTime;

    [Header("----- Weapon Stats -----")]
    [SerializeField] List<weaponStats> weaponList = new List<weaponStats>();
    [SerializeField] GameObject weaponModel;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int shootDamage;
    [SerializeField] float zoomMax;
    Vector3 muzzleFlashPosition;
    GameObject crosshair;
    Sprite crosshairTexture;

    // Deactivated temp
    // [SerializeField] int bulletSpeed;
    [SerializeField] Transform shootPositionPlayer;
    // Deactivated temp
    // [SerializeField] GameObject bullet;
    [SerializeField] GameObject gunFlash;    

    int jumpsCurrent;
    Vector3 move;
    public Vector3 playerVelocity;
    bool isShooting;
    bool isRunning;
    public int hpOriginal;
    public int speedOriginal;
    public int gunSelection;
    public float baseFOV;
    Vector3 pushback;

    //Angel ADDED THIS CODE
    public bool abilityOneActive = false;
    public bool abilityTwoActive = false;
    public bool abilityFourActive = false;
    Rigidbody rig;
    //Angel ADDED THIS CODE ABOVE


    // Start is called before the first frame update
    void Start()
    {
        hpOriginal = HP;
        speedOriginal = playerSpeed;
        baseFOV = Camera.main.fieldOfView;
        playerRespawn();
        crosshair = gameManager.instance.crosshair;


        //Angel ADDED THIS CODE
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        pushback = Vector3.Lerp(pushback, Vector3.zero, Time.deltaTime * pushbackResTime);
        movement();
        selectGun();
        zoomCamera();
        //playeranim.SetFloat("Speed", controller.velocity.normalized.magnitude);


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

        move = move.normalized;

        
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
        controller.Move((playerVelocity + pushback) * Time.deltaTime);
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
        if (weaponList.Count > 0)
        {
            gameManager.instance.muzzleFlash.GetComponent<ParticleSystem>().Play();
            yield return new WaitForSeconds(0.1f);
            gameManager.instance.muzzleFlash.GetComponent<ParticleSystem>().Stop();
        }
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
    public void weaponPickup(weaponStats weaponStat)
    {
        weaponList.Add(weaponStat);

        shootRate = weaponStat.shootRate;
        shootDist = weaponStat.shootDist;
        shootDamage = weaponStat.shootDamage;
        muzzleFlashPosition = weaponStat.muzzleFlashPosition;
        crosshairTexture = weaponStat.crosshairTexture;

        //crosshair.GetComponent<SpriteRenderer>().sprite.texture = crosshairTexture;
        gameManager.instance.muzzleFlash.transform.localPosition = muzzleFlashPosition;

        weaponModel.GetComponent<MeshFilter>().sharedMesh = weaponStat.weaponModel.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weaponStat.weaponModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && gunSelection < weaponList.Count - 1)
        {
            gunSelection++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gunSelection > 0)
        {
            gunSelection--;
            changeGun();
        }
    }

    void changeGun()
    {
        shootRate = weaponList[gunSelection].shootRate;
        shootDist = weaponList[gunSelection].shootDist;
        shootDamage = weaponList[gunSelection].shootDamage;
        muzzleFlashPosition = weaponList[gunSelection].muzzleFlashPosition;
        crosshairTexture = weaponList[gunSelection].crosshairTexture;

        //crosshair.GetComponent<Texture>().sprite.texture = crosshairTexture;
        gameManager.instance.muzzleFlash.transform.localPosition = muzzleFlashPosition;

        weaponModel.GetComponent<MeshFilter>().sharedMesh = weaponList[gunSelection].weaponModel.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weaponList[gunSelection].weaponModel.GetComponent<MeshRenderer>().sharedMaterial;        
    }

    public void playerRespawn()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPosition.transform.position;
        HP = hpOriginal;
        updatePlayerHPBar();
        controller.enabled = true;
    }

    //Angel ADDED THIS CODE
    public IEnumerator abilityCoolSpeed(float cooldown)
    {
        abilityOneActive = true;
        playerSpeed += 20;
        yield return new WaitForSeconds(cooldown);
        playerSpeed = speedOriginal;
        abilityOneActive = false;
    }

    public IEnumerator abilityCoolHeart(float cooldown)
    {
        abilityTwoActive = true;
        giveHP(1);
        yield return new WaitForSeconds(cooldown);
        abilityTwoActive = false;
    }

    public IEnumerator abilityCoolDash(float cooldown)
    {
        abilityFourActive = true;
        controller.Move(Camera.main.transform.forward * 20);
        yield return new WaitForSeconds(cooldown);
        abilityFourActive = false;
    }
    //Angel ADDED THIS CODE ABOVE

    public void pushbackDir(Vector3 dir)
    {
        pushback += dir;
    }

    public void zoomCamera()
    {
        if (weaponList.Count > 0)
        {
            if (Input.GetButton("Zoom"))
            {
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomMax, Time.deltaTime * 3);
            }
            else if (Camera.main.fieldOfView <= baseFOV)
            {
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, baseFOV, Time.deltaTime * 6);
            }
        }
    }
}
