using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// getting access to the nav mesh.
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
{
    [Header("----- Components -----")]
    // For flashing the material red "visual feedback"
    [SerializeField] Renderer model;
    // This is to attach the enemy to the nav mesh. 
    [SerializeField] NavMeshAgent agent;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int hitPoints;
    [SerializeField] int playerFaceSpeed;

    [Header("----- Gun -----")]
    [SerializeField] Transform shootPosition;
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletSpeed;
    [SerializeField] float shootRate;

    Vector3 playerDirection;
    bool isPlayerInRange;
    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        model.material.color = Color.white;
    }
}
