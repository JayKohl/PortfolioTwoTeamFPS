using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(5, 10)] [SerializeField] int HP;
    [Range(1, 5)] [SerializeField] int playerSpeed;
    [Range(1, 3)] [SerializeField] int jumpTimes;
    [Range(10, 25)] [SerializeField] int jumpSpeed;
    [Range(15, 45)] [SerializeField] int gravity;

    [Header("----- Gun Stats -----")]
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int shootDamage;
    [SerializeField] GameObject cube;

    int jumpsCurrent;
    Vector3 move;
    Vector3 playerVelocity;
    bool isShooting;
    int HPOriginal;

    // Start is called before the first frame update
    void Start()
    {
        HPOriginal = HP;
        updatePlayerHPBar();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        if (!isShooting && Input.GetButton("Shoot"))
            StartCoroutine(shoot());
    }

}
