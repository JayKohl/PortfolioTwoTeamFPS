using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] int spawnMaxNum;
    [SerializeField] int timer;
    [SerializeField] Transform[] spawnPos;

    int enemiesSpawned;
    bool playerInRange;
    bool isSpawning;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(spawnMaxNum);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && !isSpawning && enemiesSpawned < spawnMaxNum)
        {
            StartCoroutine(spawn());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;
        //int pos = Random.Range(0, spawnPos.Length);
        //Instantiate(enemy, spawnPos[pos].position, spawnPos[pos].rotation);
        Instantiate(enemy, spawnPos[Random.Range(0, spawnPos.Length)].position, enemy.transform.rotation);
        enemiesSpawned++;
        yield return new WaitForSeconds(timer);
        isSpawning = false;
    }
}

