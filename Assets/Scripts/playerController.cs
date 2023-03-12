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
    [SerializeField] public AudioSource aud;
    [SerializeField] AudioClip medPickupSound;
    [Range(0, 1)] [SerializeField] float medPickupVol;
    [SerializeField] AudioClip weaponPickupSound;
    [Range(0, 1)] [SerializeField] float weaponPickupVol;
    [SerializeField] public GameObject minimap;
    [Header("----- Player Stats -----")]
    [Range(5, 30)] [SerializeField] public int HP;
    [Range(1, 50)] [SerializeField] public float playerSpeed;
    [Range(1, 3)] [SerializeField] public int jumpTimes;
    [Range(10, 25)] [SerializeField] int jumpSpeed;
    [Range(15, 45)] [SerializeField] int gravity;
    [SerializeField] public float runSpeed;
    [SerializeField] float pushbackResTime;
    [SerializeField] int crouchSpeed;
    [SerializeField] float crouchHeight;

    [Header("----- Weapon Stats -----")]
    [SerializeField] List<weaponStats> weaponList = new List<weaponStats>();
    [SerializeField] public GameObject weaponModel;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] public int shootDamage;
    [SerializeField] float zoomMax;
    Vector3 muzzleFlashPosition;
    [SerializeField] public GameObject shieldOnPlayer;
    [SerializeField] public GameObject fireOnPlayer;
    [SerializeField] public GameObject iceOnPlayer;
    [SerializeField] GameObject crosshair;
    Sprite crosshairTexture;
    [SerializeField] GameObject weaponIcon;
    [SerializeField] float grenadeYVel;
    [SerializeField] int grenadeSpeed;
    [SerializeField] GameObject grenade;
    [SerializeField] GameObject fireflies;
    [SerializeField] GameObject gravityBomb;
    [SerializeField] GameObject sentryGun;
    [SerializeField] string weaponName;
    [SerializeField] public List<AudioClip> weaponAudio = new List<AudioClip>();
    [Range(0, 1)] [SerializeField] float weaponAudioVol;
    public MeshRenderer visible;
    public int weaponDamageMulti = 1;
    public int dmgDivide = 1;

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

    [SerializeField] AudioClip[] audDead;
    [Range(0, 1)] [SerializeField] float audDeadVol;

    [SerializeField] public AudioClip lvlUp;
    [Range(0, 1)] [SerializeField] public float lvlUpVol;

    public bool dirt;



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
    public float speedOriginal;
    public int gunSelection;
    public float baseFOV;
    Vector3 pushback;
    bool isPlayingSteps;
    bool isSprinting;
    public bool isCrouched;
    public float startY;

    public bool abilityOneActive = false;
    public bool abilityTwoActive = false;
    public bool abilityThreeActive = false;
    public bool abilityFourActive = false;
    Rigidbody rig;
    public bool fireOn;
    public bool iceOn;
    public bool canShoot = true;

    int currentLevel = 0;


    public bool slowed;
    public bool electrecuted;
    public bool poisoned;
    public bool burning;

    public bool playerDied;
    // Start is called before the first frame update
    void Start()
    {
       
        canShoot = true;
        if (SceneManager.GetActiveScene().name == "LvlOneArena" && currentLevel < 1)
        {
            dirt = true;
            currentLevel = 1;
            gameManager.instance.fuelCellsRemainingObject.SetActive(true);
            gameManager.instance.enemiesRemainingObject.SetActive(false);
            gameManager.instance.infoTextBackground.SetActive(false);
            gameManager.instance.infoText.text = "";
        }
        else if (SceneManager.GetActiveScene().name == "LvlTwoTheArena" && currentLevel < 2)
        {
            dirt = false;
            currentLevel = 2;
            gameManager.instance.enemiesRemainingObject.SetActive(true);
            gameManager.instance.fuelCellsRemainingObject.SetActive(false);
            gameManager.instance.infoTextBackground.SetActive(false);
            gameManager.instance.infoText.text = "";
        }
        else if (SceneManager.GetActiveScene().name == "LvlThreeTheWorld" && currentLevel < 3)
        {
            dirt = true;
            currentLevel = 3;
            gameManager.instance.enemiesRemainingObject.SetActive(false);
            gameManager.instance.fuelCellsRemainingObject.SetActive(false);
            gameManager.instance.infoText.text = "Investigate the town to find the source of the distress signal.";
            gameManager.instance.infoTextBackground.SetActive(true);
        }
        weaponIcon = GameObject.FindGameObjectWithTag("Weapon Icon");
        crosshair = GameObject.FindGameObjectWithTag("Crosshair");
        if (weaponList.Count == 0)
            weaponIcon.SetActive(false);
        hpOriginal = HP;
        speedOriginal = playerSpeed;
        baseFOV = Camera.main.fieldOfView;
        startY = transform.localScale.y;
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


        if (!isShooting && Input.GetButton("Shoot") && canShoot && gameManager.instance.activeMenu == null)
        {
            //Debug.Log("test1");
            if (weaponList.Count > 0)
            {
                isShooting = true;
                StartCoroutine(shoot());
            }
        }
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
        if (Input.GetButton("Crouch"))
        {
            isCrouched = true;

        }
        if (Input.GetButtonDown("Jump") && jumpsCurrent < jumpTimes)
        {
            jumpsCurrent++;
            playerVelocity.y = jumpSpeed;
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
        }
        if ((Input.GetButton("Run") && !isRunning) && !isCrouched) //run
        {
            isRunning = true;
            playerSpeed = runSpeed;
            //aud.PlayOneShot(audGravelRun[Random.Range(0, audGravelRun.Length)], audGravelRunVol);
        }
        if (isRunning && Input.GetButtonUp("Run")) //walk
        {
            isRunning = false;
            playerSpeed = speedOriginal;
            //aud.PlayOneShot(audGravelSteps[Random.Range(0, audGravelSteps.Length)], audGravelStepsVol);
        }
        if (Input.GetButtonDown("Crouch") && !isCrouched)
        {
            isCrouched = true;
            playerSpeed = crouchSpeed;
            //transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
            controller.height = crouchHeight;
            //rig.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        }
        if (Input.GetButtonUp("Crouch") && isCrouched)
        {
            isCrouched = false;
            playerSpeed = speedOriginal;
            transform.localScale = new Vector3(transform.localScale.x, startY, transform.localScale.z);


        }
        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move((playerVelocity + pushback) * Time.deltaTime);

        if (controller.isGrounded && move.normalized.magnitude > 0.8f && !isPlayingSteps)
        {
            if (dirt)
            {
                StartCoroutine(playGravelSteps());
            }
            else
            {
                StartCoroutine(playMetalSteps());
            }
        }
        
    }

    public void updateGoals(string goal)
    {
        gameManager.instance.infoText.text = goal;
        if (goal == "")
        {
            gameManager.instance.infoTextBackground.SetActive(false);
        }
        else
        {
            gameManager.instance.infoTextBackground.SetActive(true);
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
        //Debug.Log("test2");
        // Control for isShooting animation bool
        if (gameManager.instance.activeMenu == null)
        {
            if (weaponName == "Pistol")
            {
                playeranim.SetBool("Pistol", true);
                playeranim.SetBool("SMG", false);
                playeranim.SetBool("MachineGun", false);
                playeranim.SetBool("Rifle", false);
                playeranim.SetBool("Sniper", false);
            }
            else if (weaponName == "SMG")
            {
                playeranim.SetBool("Pistol", false);
                playeranim.SetBool("SMG", true);
                playeranim.SetBool("MachineGun", false);
                playeranim.SetBool("Rifle", false);
                playeranim.SetBool("Sniper", false);
            }
            else if (weaponName == "MachineGun")
            {
                playeranim.SetBool("Pistol", false);
                playeranim.SetBool("SMG", false);
                playeranim.SetBool("MachineGun", true);
                playeranim.SetBool("Rifle", false);
                playeranim.SetBool("Sniper", false);
            }
            else if (weaponName == "Rifle")
            {
                playeranim.SetBool("Pistol", false);
                playeranim.SetBool("SMG", false);
                playeranim.SetBool("MachineGun", false);
                playeranim.SetBool("Rifle", true);
                playeranim.SetBool("Sniper", false);
            }
            else if (weaponName == "Sniper")
            {
                playeranim.SetBool("Pistol", false);
                playeranim.SetBool("SMG", false);
                playeranim.SetBool("MachineGun", false);
                playeranim.SetBool("Rifle", false);
                playeranim.SetBool("Sniper", true);
            }
            aud.PlayOneShot(weaponAudio[Random.Range(0, weaponAudio.Count)], weaponAudioVol);
            StartCoroutine(gunShootFlash());
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            //Debug.Log(hit.collider.name);


            // Deactivated temp
            // GameObject bulletClone = Instantiate(bullet, shootPositionPlayer.position, bullet.transform.rotation);
            // bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

            if (hit.collider.GetComponent<IDamage>() != null)
            {
                gameObject.tag = "Player";
                gameManager.instance.invisUI.SetActive(false);
                weaponModel.GetComponent<MeshRenderer>().enabled = true;
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
    public void swarm()
    {
        GameObject bulletClone = Instantiate(fireflies, shootPositionPlayer.position, grenade.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = (transform.forward + new Vector3(0, grenadeYVel, 0)) * grenadeSpeed;
    }
    public void gravBomb()
    {
        GameObject bulletClone = Instantiate(gravityBomb, shootPositionPlayer.position, grenade.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = (transform.forward + new Vector3(0, grenadeYVel, 0)) * grenadeSpeed;
    }
    public void deploySentryGun()
    {
        GameObject bulletClone = Instantiate(sentryGun, shootPositionPlayer.position, sentryGun.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }
    public void takeDamage(int dmg)
    {
        if (gameManager.instance.shieldOn)
        {
            shieldOnPlayer.GetComponent<shield>().shieldTakeDamage(dmg);
        }
        else
        {
            if (dmg / dmgDivide > 0)
            {
                StartCoroutine(gameManager.instance.abilityHub.GetComponent<activateAbility>().beginHack(5));
                HP -= (dmg / dmgDivide);
                updatePlayerHPBar();
                StartCoroutine(flashDamage());
            }
            else
            {
                HP -= 1;
                updatePlayerHPBar();
                StartCoroutine(flashDamage());
            }

            if (HP <= 0)
            {
                playerDied = true;
                aud.PlayOneShot(audDead[Random.Range(0, audDead.Length)], audDeadVol);
                gameManager.instance.playerDead();
            }
            else
            {
                aud.PlayOneShot(audDamaged[Random.Range(0, audDamaged.Length)], audDamagedVol);
            }
        }
    }
    public void invisibility()
    {
        gameObject.tag = "Invisible";
        weaponModel.GetComponent<MeshRenderer>().enabled = false;
        gameManager.instance.invisUI.SetActive(true);
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
    IEnumerator flashDamage(int type = 0)

    {
        switch (type)
        {
            case 0:
                if (!gameManager.instance.shieldOn)
                {
                    gameManager.instance.playerDamageFlashScreen.SetActive(true);
                    yield return new WaitForSeconds(0.1f);
                    gameManager.instance.playerDamageFlashScreen.SetActive(false);
                }
                break;
            default:
                break;
        }



    }

    public void giveHP(int amount)
    {
        aud.PlayOneShot(medPickupSound, medPickupVol);
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
        for (int i = 0; i < weaponList.Count; i++)
        {
            if (weaponStat == weaponList[i])
            {
                aud.PlayOneShot(weaponPickupSound, weaponPickupVol);
                return;
            }
        }
        aud.PlayOneShot(weaponPickupSound, weaponPickupVol);
        weaponList.Add(weaponStat);

        shootRate = weaponStat.shootRate;
        shootDist = weaponStat.shootDist;
        shootDamage = weaponStat.shootDamage * weaponDamageMulti;
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
        shootDamage = weaponList[gunSelection].shootDamage * weaponDamageMulti;
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

    public IEnumerator abilityCoolInvisible(float cooldown)
    {
        abilityThreeActive = true;
        yield return new WaitForSeconds(cooldown);
        gameObject.tag = "Player";
        gameManager.instance.invisUI.SetActive(false);
        weaponModel.GetComponent<MeshRenderer>().enabled = true;
        abilityThreeActive = false;
    }

    public IEnumerator abilityCoolDash(float cooldown)
    {
        abilityFourActive = true;
        controller.Move(Camera.main.transform.forward * 20);
        gameManager.instance.dashUI.SetActive(true);
        yield return new WaitForSeconds(.1f);
        gameManager.instance.dashUI.SetActive(false);
        yield return new WaitForSeconds(cooldown);
        abilityFourActive = false;
    }

    public void pushbackDir(Vector3 dir)
    {
        pushback += dir;
    }

    public void zoomCamera()
    {
        if (gameManager.instance.cam2.GetComponentInChildren<Camera>().enabled)
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

    public void animatePlayer()
    {
        // Control for isWalking animation bool
        if (Input.GetKey("w") || Input.GetKey("s"))
        {
            playeranim.SetBool("isWalking", true);
        }
        if (!Input.GetKey("w") && !Input.GetKey("s"))
        {
            playeranim.SetBool("isWalking", false);
        }

        // Control for isRunning animation bool
        if (((Input.GetKey("w") || Input.GetKey("s")) && Input.GetKey("left shift")) && !isCrouched)
        {
            playeranim.SetBool("isRunning", true);
        }
        if ((!Input.GetKey("w") && !Input.GetKey("s")) || !Input.GetKey("left shift"))
        {
            playeranim.SetBool("isRunning", false);
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
            playeranim.SetBool("isJumping", true);
        }
        if (controller.isGrounded)
        {
            playeranim.SetBool("isJumping", false);
        }

    }


    //Added code
    public IEnumerator Poisoned(int effectTime, int trapDamage, AudioClip effect)
    {
       
        while (effectTime > 0 && !playerDied)
        {     
            takeDamage(trapDamage);
            StartCoroutine(flashDamage());
            aud.PlayOneShot(audDamaged[Random.Range(0, audDamaged.Length)], audDamagedVol);
            effectTime--;
            if(playerDied)
            {
                yield break;
            }
         
            yield return new WaitForSeconds(1.5f);
        }
        
        poisoned = false;
    }

    public void Burning(int effectTime, int trapDamage, AudioClip effect)
    {
        while (effectTime > 0)
        {
            takeDamage(trapDamage);
            new WaitForSeconds(1);
            StartCoroutine(flashDamage());
            aud.PlayOneShot(effect);
        }
    }

    public IEnumerator Slowed(int effectTime, int trapDamage)
    {

        playerSpeed = trapDamage;
        yield return new WaitForSeconds(effectTime);
        playerSpeed = speedOriginal;
    }

    public void Electrecuted(int effectTime, int trapDamage, AudioClip effect)
    {
        while (effectTime > 0)
        {
            takeDamage(trapDamage);
            new WaitForSeconds(1);
            StartCoroutine(flashDamage());
            aud.PlayOneShot(effect);
        }
    }

    IEnumerator waitSeconds()
    {
        yield return new WaitForSeconds(1.5f);
    }


}
