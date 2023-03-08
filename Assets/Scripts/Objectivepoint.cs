using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objectivepoint : MonoBehaviour
{
    public Image locator;
    public Transform location;
    GameObject nullLoc;
    public Transform objectiveOne;
    private void Start()
    {
        locator.enabled = false;
        nullLoc = GameObject.FindGameObjectWithTag("NullObjective");
        location = nullLoc.transform;
    }
    void Update()
    {
        float minX = locator.GetPixelAdjustedRect().width / 2;
        float manX = Screen.width - minX;

        float minY = locator.GetPixelAdjustedRect().width / 2;
        float manY = Screen.height - minX;

        Vector2 pos = Camera.main.WorldToScreenPoint(location.position);

        if (Vector3.Dot((location.position - transform.position), transform.forward) < 0)
        {
            if (pos.x < Screen.width / 2)
            {
                pos.x = manX;
            }
            else
            {
                pos.x = minX;
            }
        }
           
        pos.x = Mathf.Clamp(pos.x, minX, manX);
        pos.y = Mathf.Clamp(pos.y, minX, manX);
        locator.transform.position = pos;
    }
}
