using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapRadar : MonoBehaviour
{
    public Transform player;

    private void LateUpdate()
    {
        Vector3 position = player.position;
        position.y = transform.position.y;
        transform.position = position;
        player = gameManager.instance.player.transform;
        transform.rotation = Quaternion.Euler(90f, player.localEulerAngles.y, 0);
    }
}
