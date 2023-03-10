using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossEvent : MonoBehaviour
{
    public bool bossKill;
    [SerializeField] GameObject bossAreaEntrance;
    [SerializeField] BoxCollider toggleMesh;
    [SerializeField] GameObject bossAttachedToDoor;
    [SerializeField] Renderer setLazerColor;
    [SerializeField] Renderer setLazerColorTwo;
    [SerializeField] Renderer setLazerColorThree;
    [SerializeField] Renderer setLazerColorFour;
    [SerializeField] GameObject preventRunning;

    // Start is called before the first frame update
    void Start()
    {
        bossKill = false;
        setLazerColor.material.color = Color.clear;
        setLazerColorTwo.material.color = Color.clear;
        setLazerColorThree.material.color = Color.clear;
        setLazerColorFour.material.color = Color.clear;
    }

    void Update()
    {
        if (bossAttachedToDoor.GetComponent<enemyBossAI>().hitPoints <= 0)
        {
            setLazerColor.material.color = Color.clear;
            setLazerColorTwo.material.color = Color.clear;
            setLazerColorThree.material.color = Color.clear;
            setLazerColorFour.material.color = Color.clear;
            bossKill = true;
            toggleMesh.enabled = false;
        }
    }
    IEnumerator toggleDoorLock()
    {
        yield return new WaitForSeconds(.15f);
        toggleMesh.enabled = true;//toggleMesh;
        gameManager.instance.playerScript.controller.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!bossKill)
        {
            gameManager.instance.playerScript.controller.enabled = false;
            gameManager.instance.player.transform.position = preventRunning.transform.position;
            StartCoroutine(toggleDoorLock());

            setLazerColor.material.color = Color.white;
            setLazerColorTwo.material.color = Color.white;
            setLazerColorThree.material.color = Color.white;
            setLazerColorFour.material.color = Color.white;
        }
    }
}
