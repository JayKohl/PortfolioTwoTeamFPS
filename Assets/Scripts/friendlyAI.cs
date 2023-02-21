using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class friendlyAI : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    public NavMeshAgent agent;
    [SerializeField] Animator anim;

    [Header("----- NPC Stats -----")]
    [SerializeField] Transform headPos;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int waitTime;
    [SerializeField] int roamDist;

    bool isPlayerInRange;
    float angleToPlayer;
    Vector3 startingPos;
    Vector3 moveToTerminal;
    bool destinationChosen;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
            if (true)
            {

            }
        }
    }
}
