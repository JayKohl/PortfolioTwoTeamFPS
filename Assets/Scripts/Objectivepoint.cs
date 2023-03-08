using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objectivepoint : MonoBehaviour
{
    public Image locator;
    public Transform location;
    void Update()
    {
        locator.transform.position = Camera.main.WorldToScreenPoint(location.position);
    }
}
