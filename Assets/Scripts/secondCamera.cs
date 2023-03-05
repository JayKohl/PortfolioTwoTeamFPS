using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class secondCamera : MonoBehaviour
{
    [SerializeField] public Transform locDoorOne;
    [SerializeField] public Transform locDoorTwo;
    [SerializeField] public GameObject doorOne;
    [SerializeField] public GameObject doorTwo;
    public Camera cam2;

    public void openDoorOne()
    {
        transform.position = locDoorOne.transform.position;
        transform.eulerAngles = locDoorOne.transform.eulerAngles;
        cam2.fieldOfView = Mathf.Lerp(cam2.fieldOfView, 50, 40);

        //gameManager.instance.cam2.fieldOfView = Mathf.Lerp(gameManager.instance.cam2.fieldOfView, 80, 4 * Time.unscaledDeltaTime);
    }

    public void openDoorTwo()
    {
        transform.position = locDoorTwo.transform.position;
        transform.eulerAngles = locDoorTwo.transform.eulerAngles;
        cam2.fieldOfView = Mathf.Lerp(cam2.fieldOfView, 50, 40 * Time.deltaTime);
        //gameManager.instance.cam2.fieldOfView = Mathf.Lerp(gameManager.instance.cam2.fieldOfView, 80, 4 * Time.unscaledDeltaTime);

    }
}