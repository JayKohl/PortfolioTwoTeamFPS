using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyThirdBoss : enemyAI
{
    [SerializeField] GameObject topPiece;
    bool isFlip;
    // Start is called before the first frame update
    void Start()
    {
        agentStop();
        isFlip = false;
        Vector3 StartingPos = topPiece.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {

        topPiece.transform.Rotate(0f, 1f, 0f, Space.Self);
        if (!isFlip)
        {
            flipObject();
        }
    }
    private void flipObject()
    {
        isFlip = true;
        topPiece.transform.Rotate(180f, 0f, 0f, Space.Self);
    }
}
