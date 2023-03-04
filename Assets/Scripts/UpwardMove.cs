using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpwardMove : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0, 2, 0), 1);
        }
    }
}
