using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyParticlesOnTimer : MonoBehaviour
{
    [SerializeField] int timer;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(timerStart());
    }
    IEnumerator timerStart()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
