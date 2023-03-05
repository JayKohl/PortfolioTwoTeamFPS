using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyThirdBoss : MonoBehaviour
{
    [SerializeField] GameObject topPiece;

    // Start is called before the first frame update
    //void Start()
    //{
    //}

    // Update is called once per frame
    void Update()
    {
        topPiece.transform.Rotate(0f, 1f, 0f, Space.Self);
    }
}
