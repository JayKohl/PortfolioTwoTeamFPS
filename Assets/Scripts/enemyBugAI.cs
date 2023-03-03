using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class enemyBugAI : enemyAI
{
    [SerializeField] protected Collider meleeColliderTwo;
    [SerializeField] Collider meleeColliderRam;

    int hitPointsOrig;
    bool isAgro;

    System.Random randomAttack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
