using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openingDoors : MonoBehaviour
{
    [SerializeField] Transform doorCamOne;

    private void Start()
    {
        
    }
    public IEnumerator OpenDoorOne(GameObject door)
    {
        door.SetActive(false);
        gameManager.instance.cam2.transform.position = doorCamOne.position;

        //transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0, -10, 0), 10f) * Time.deltaTime;
        yield return new WaitForSeconds(.5f);
    }
}
