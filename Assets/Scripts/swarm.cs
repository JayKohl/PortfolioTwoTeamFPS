using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swarm : MonoBehaviour
{
    [SerializeField] float time = 5;
    private void Start()
    {
        StartCoroutine(timerDeath());
    }
    IEnumerator timerDeath()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
