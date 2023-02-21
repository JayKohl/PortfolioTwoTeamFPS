using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyParticlesOnTimer : MonoBehaviour
{
    [SerializeField] int time;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(timer());
    }
    IEnumerator timer()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
