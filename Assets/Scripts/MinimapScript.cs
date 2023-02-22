using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    [SerializeField] Transform radar;
    float radarSpeed;
    public Transform player;
    void Awake()
    {
        radarSpeed = 180f;
    }
    private void LateUpdate()
    {
        Vector3 cameraPosition = player.position;
        cameraPosition.y = transform.position.y;
        transform.position = cameraPosition;

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0);
        radar.eulerAngles -= new Vector3(0, 0, radarSpeed * Time.deltaTime);
    }
}
