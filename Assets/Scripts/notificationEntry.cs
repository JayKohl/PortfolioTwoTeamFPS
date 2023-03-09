using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class notificationEntry : MonoBehaviour
{
    [SerializeField] notifications texture;
    bool playerIn;
    private void Start()
    {
        playerIn = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerIn)
        {
            playerIn = true;
            StartCoroutine(gameManager.instance.notificationDisplay(texture));
            gameManager.instance.playerSpawnPosition.transform.position = transform.position;
            StartCoroutine(delete());
        }
    }
    IEnumerator delete()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}