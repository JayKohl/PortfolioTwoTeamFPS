using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abilityPickup : MonoBehaviour
{
    bool playerIn;
    [SerializeField] abilities stats;

    private void Start()
    {
        StartCoroutine(rotate());
    }
    private IEnumerator rotate()
    {
        while (true)
        {
            if (!gameManager.instance.isPaused)
            {
                transform.Rotate(0f, 0.5f, 0f, Space.Self);
            }
            yield return new WaitForSeconds(1f / 60);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerIn)
        {
            playerIn = true;
            gameManager.instance.abilityHub.GetComponent<activateAbility>().abilityPickup(stats);
            gameManager.instance.pause();
            Destroy(gameObject);
        }
    }
}