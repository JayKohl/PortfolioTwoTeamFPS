using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControls : MonoBehaviour
{
    [SerializeField] bool invertX;

    [SerializeField] int HorA;
    [SerializeField] int VerA;
    [SerializeField] int VerMin;
    [SerializeField] int VerMax;

    float xRotation;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVer;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHor;

        if (invertX)
            xRotation += mouseY;
        else
            xRotation -= mouseY;
       
        xRotation = Mathf.Clamp(xRotation, VerMin, VerMax);

        //rotate on X-axis
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //rotate on y-axis
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
