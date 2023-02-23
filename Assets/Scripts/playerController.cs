using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] public CharacterController controller;
    [SerializeField] Animator playeranim;
    [SerializeField] AudioSource aud;

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
    [SerializeField] public GameObject shieldOnPlayer;
    [SerializeField] GameObject crosshair;
    Sprite crosshairTexture;
    [SerializeField] GameObject weaponIcon;
    [SerializeField] float grenadeYVel;
    [SerializeField] int grenadeSpeed;
    [SerializeField] GameObject grenade;
    [SerializeField] string weaponName;
    [SerializeField] AudioClip weaponAudio;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audGravelSteps;
    [Range(0, 1)] [SerializeField] float audGravelStepsVol;
    [SerializeField] AudioClip[] audGravelRun;
    [Range(0, 1)] [SerializeField] float audGravelRunVol;

    [SerializeField] AudioClip[] audMetalSteps;
    [Range(0, 1)] [SerializeField] float audMetalStepsVol;
    [SerializeField] AudioClip[] audMetalRun;
    [Range(0, 1)] [SerializeField] float audMetalRunVol;

    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)] [SerializeField] float audJumpVol;

    [SerializeField] AudioClip[] audDamaged;
    [Range(0, 1)] [SerializeField] float audDamagedVol;



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
    bool isPlayingSteps;
    bool isSprinting;

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
        if (SceneManager.GetActiveScene().name == "LvlOneArena")
        {
            gameManager.instance.fuelCellsRemainingObject.SetActive(true);
            gameManager.instance.enemiesRemainingObject.SetActive(false);
            
        }
        if (SceneManager.GetActiveScene().name == "LvlTwoTheArena")
        {
            gameManager.instance.enemiesRemainingObject.SetActive(true);
            gameManager.instance.fuelCellsRemainingObject.SetActive(false);
        }
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
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
        }
        if (Input.GetButton("Run") && !isRunning) //run
        {
            isRunning = true;
            playerSpeed = runSpeed;
            aud.PlayOneShot(audGravelRun[Random.Range(0, audGravelRun.Length)], audGravelRunVol);
        }
        if (isRunning && Input.GetButtonUp("Run")) //walk
        {
            isRunning = false;
            playerSpeed = speedOriginal;
            aud.PlayOneShot(audGravelSteps[Random.Range(0, audGravelSteps.Length)], audGravelStepsVol);
        }
        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move((playerVelocity + pushback) * Time.deltaTime);

        if (controller.isGrounded && move.normalized.magnitude > 0.8f && !isPlayingSteps)
        {
            StartCoroutine(playGravelSteps());
        }
    }

    IEnumerator playGravelSteps()
    {
        isPlayingSteps = true;
        aud.PlayOneShot(audGravelSteps[Random.Range(0, audGravelSteps.Length)], audGravelStepsVol);
        if (isSprinting)
        {
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        isPlayingSteps = false;
    }

    IEnumerator playMetalSteps()
    {
        isPlayingSteps = true;
        aud.PlayOneShot(audMetalSteps[Random.Range(0, audMetalSteps.Length)], audMetalStepsVol);
        if (isSprinting)
        {
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        isPlayingSteps = false;
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
            aud.PlayOneShot(audDamaged[Random.Range(0, audDamaged.Length)], audDamagedVol);

            if (HP <= 0)
                gameManager.instance.playerDead();
        }
    }
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
        weaponAudio = weaponStat.weaponAudio;

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
        weaponAudio = weaponList[gunSelection].weaponAudio;

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
        yield return new WaitForSeconds(cooldown);
        if (gameManager.instance.shieldOn)
        {
            shieldOnPlayer.GetComponent<shield>().timeOver();
        }
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
