using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abilityPickup : MonoBehaviour
{
    float time = 1;
    bool playerIn;
    [Range(1, 4)] [SerializeField] int abilityNumber;

    void Update()
    {
        StartCoroutine(bounce());
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
            if (abilityNumber == 1)
            {
                gameManager.instance.AbilitiesBackground.SetActive(true);
                gameManager.instance.AbilityOne.SetActive(true);
            }
            else if (abilityNumber == 2)
            {
                gameManager.instance.AbilitiesBackground.SetActive(true);
                gameManager.instance.AbilityTwo.SetActive(true);
            }
            else if (abilityNumber == 3)
            {
                gameManager.instance.AbilitiesBackground.SetActive(true);
                gameManager.instance.AbilityThree.SetActive(true);
            }
            else if (abilityNumber == 4)
            {
                gameManager.instance.AbilitiesBackground.SetActive(true);
                gameManager.instance.AbilityFour.SetActive(true);
            }
            Destroy(gameObject);
        }
    }
}