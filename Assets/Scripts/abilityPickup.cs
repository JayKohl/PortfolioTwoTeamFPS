using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abilityPickup : MonoBehaviour
{
    float time = 1;
    bool playerIn;
    [SerializeField] abilities stats;

    void Update()
    {
        //StartCoroutine(bounce());
    }
    IEnumerator bounce()
    {
        if (!gameManager.instance.isPaused)
        {
            transform.Translate(0, 0.002f, 0, Space.World);
            yield return new WaitForSeconds(time);
            transform.Translate(0, -0.002f, 0, Space.World);
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