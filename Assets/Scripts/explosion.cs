using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] public int missileDamage;
    [SerializeField] int pushBackDistance;
    bool playerIn;
    private void Start()
    {
        StartCoroutine(timer(.5f));
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !playerIn)
        {
            playerIn = true;
            gameManager.instance.playerScript.takeDamage(missileDamage);
            gameManager.instance.playerScript.pushbackDir((gameManager.instance.player.transform.position - transform.position).normalized * pushBackDistance);
        }
    }
    IEnumerator timer(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
