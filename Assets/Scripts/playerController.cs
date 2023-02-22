using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] GameObject shieldOnPlayer;
    [SerializeField] GameObject crosshair;
    Sprite crosshairTexture;
    [SerializeField] GameObject weaponIcon;
    [SerializeField] float grenadeYVel;
    [SerializeField] int grenadeSpeed;
    [SerializeField] GameObject grenade;
    [SerializeField] string weaponName;

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

    public bool abilityOneActive = false;
    public bool abilityTwoActive = false;
    public bool abilityThreeActive = false;
    public bool abilityFourActive = false;
    Rigidbody rig;


    // Start is called before the first frame update
    void Start()
    {
        weaponIcon = GameObject.FindGameObjectWithTag("Weapon Icon");
        crosshair = GameObject.FindGameObjectWithTag("Crosshair");
        if (weaponList.Count == 0)
            weaponIcon.SetActive(false);
        hpOriginal = HP;
        speedOriginal = playerSpeed;
        baseFOV = Camera.main.fieldOfView;
        playerRespawn();
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        pushback = Vector3.Lerp(pushback, Vector3.zero, Time.deltaTime * pushbackResTime);
        movement();
        selectGun();
        zoomCamera();
        animatePlayer();
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
        if (Input.GetButton("Run") && !isRunning)
        {
            isRunning = true;
            playerSpeed = runSpeed;
        }
        if (isRunning && Input.GetButtonUp("Run"))
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
    public void throwGrenade()
    {
        GameObject bulletClone = Instantiate(grenade, shootPositionPlayer.position, grenade.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = (transform.forward + new Vector3(0, grenadeYVel, 0)) * grenadeSpeed;
    }
    public void takeDamage(int dmg)
    {
        if (gameManager.instance.shieldOn)
        {
            shieldOnPlayer.GetComponent<shield>().shieldTakeDamage(dmg);
        }
        else
        {
            HP -= dmg;
            updatePlayerHPBar();
            StartCoroutine(flashDamage());

            if (HP <= 0)
                gameManager.instance.playerDead();
        }
    }
    //public void shieldOffPlayer()
    //{
    //    gameManager.instance.shieldOn = false;
    //    gameManager.instance.shieldUI.SetActive(false);
    //}
    //public void shieldStartPlayer()
    //{
    //    abilityTwoActive = true;
    //    shieldOnPlayer.GetComponent<shield>().shieldStart();
    //}
    public void invisibility()
    {
        gameObject.tag = "Invisible";
        StartCoroutine(abilityCoolInvisible(10));
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
        if (!gameManager.instance.shieldOn)
        {
            gameManager.instance.playerDamageFlashScreen.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            gameManager.instance.playerDamageFlashScreen.SetActive(false);
        }
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
        zoomMax = weaponStat.zoomAmount;
        weaponName = weaponStat.weaponName;

        crosshair.GetComponent<Image>().sprite = crosshairTexture;
        weaponIcon.GetComponent<Image>().sprite = weaponStat.weaponIcon;
        weaponIcon.SetActive(true);
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
        zoomMax = weaponList[gunSelection].zoomAmount;
        weaponName = weaponList[gunSelection].weaponName;
        weaponIcon.GetComponent<Image>().sprite = weaponList[gunSelection].weaponIcon;

        crosshair.GetComponent<Image>().sprite = crosshairTexture;
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

    public IEnumerator abilityCoolSpeed(float cooldown)
    {
        abilityOneActive = true;
        playerSpeed += 20;
        yield return new WaitForSeconds(cooldown);
        playerSpeed = speedOriginal;
        abilityOneActive = false;
    }

    public IEnumerator abilityCoolShield(float cooldown)
    {
        shieldOnPlayer.GetComponent<shield>().shieldStart();
        yield return new WaitForSeconds(shieldOnPlayer.GetComponent<shield>().shielTimer);
        shieldOnPlayer.GetComponent<shield>().timeOver();
    }
    public IEnumerator abilityCoolInvisible(float cooldown)
    {
        abilityThreeActive = true;
        yield return new WaitForSeconds(cooldown);
        gameObject.tag = "Player";
        abilityThreeActive = false;
    }

    public IEnumerator abilityCoolDash(float cooldown)
    {
        abilityFourActive = true;
        controller.Move(Camera.main.transform.forward * 20);
        yield return new WaitForSeconds(cooldown);
        abilityFourActive = false;
    }    

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

    public void animatePlayer()
    {
        // Control for isWalking animation bool
        if(Input.GetKey("w") || Input.GetKey("s"))
        {
            playeranim.SetBool("isWalking", true);
        }
        if (!Input.GetKey("w") && !Input.GetKey("s"))
        {
            playeranim.SetBool("isWalking", false);
        }

        // Control for isRunning animation bool
        if ((Input.GetKey("w") || Input.GetKey("s")) && Input.GetKey("left shift"))
        {
            playeranim.SetBool("isRunning", true);
        }
        if ((!Input.GetKey("w") && !Input.GetKey("s")) || !Input.GetKey("left shift"))
        {
            playeranim.SetBool("isRunning", false);
        }

        // Control for isShooting animation bool
        if (weaponList.Count > 0)
       {
            if (weaponName == "Pistol")
            {
                playeranim.SetBool("Pistol", true);
                playeranim.SetBool("SMG", false);
                playeranim.SetBool("MachineGun", false);
                playeranim.SetBool("Rifle", false);
                playeranim.SetBool("Sniper", false);
            }
            if (weaponName == "SMG")
            {
                playeranim.SetBool("Pistol", false);
                playeranim.SetBool("SMG", true);
                playeranim.SetBool("MachineGun", false);
                playeranim.SetBool("Rifle", false);
                playeranim.SetBool("Sniper", false);
            }
            if (weaponName == "MachineGun")
            {
                playeranim.SetBool("Pistol", false);
                playeranim.SetBool("SMG", false);
                playeranim.SetBool("MachineGun", true);
                playeranim.SetBool("Rifle", false);
                playeranim.SetBool("Sniper", false);
            }
            if (weaponName == "Rifle")
            {
                playeranim.SetBool("Pistol", false);
                playeranim.SetBool("SMG", false);
                playeranim.SetBool("MachineGun", false);
                playeranim.SetBool("Rifle", true);
                playeranim.SetBool("Sniper", false);
            }
            if (weaponName == "Sniper")
            {
                playeranim.SetBool("Pistol", false);
                playeranim.SetBool("SMG", false);
                playeranim.SetBool("MachineGun", false);
                playeranim.SetBool("Rifle", false);
                playeranim.SetBool("Sniper", true);
            }

        }

        if (Input.GetKey("mouse 0"))
        {
            playeranim.SetBool("isShooting", true);
            

        }
        if (!Input.GetKey("mouse 0"))
        {
            playeranim.SetBool("isShooting", false);
        }

        if (Input.GetKey("space") && jumpsCurrent < jumpTimes)
        {
            playeranim.SetBool("isJummping", true);
        }
        if (controller.isGrounded)
        {
            playeranim.SetBool("isJummping", false);
        }
    }
}
