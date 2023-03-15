using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Objectivepoint : MonoBehaviour
{
    public Image locator;
    public Transform location;
    GameObject nullLoc;
    public Transform objectiveOne;
    [SerializeField] Transform townNpc;
    [SerializeField] public Transform quest2;
    [SerializeField] public Transform quest3;
    [SerializeField] public Transform quest4;
    [SerializeField] public Transform quest5;


    private void Start()
    {
        //locator = GameObject.FindGameObjectWithTag("Waypoint Image").GetComponent<Image>();

        nullLoc = GameObject.FindGameObjectWithTag("NullObjective");
        location = nullLoc.transform;
        if (SceneManager.GetActiveScene().name == "LvlOneArena")
        {
            location = nullLoc.transform;
            objectiveOne = nullLoc.transform;
            TurnOffQuest();
            townNpc = nullLoc.transform;
            quest2 = nullLoc.transform;
            quest3 = nullLoc.transform;
            quest4 = nullLoc.transform;
            quest5 = nullLoc.transform;
        }
        else if (SceneManager.GetActiveScene().name == "LvlThreeTheWorld")
        {
            SetWayPoint(townNpc);
        }
        else
        {
            TurnOffQuest();
            townNpc = nullLoc.transform;
            quest2 = nullLoc.transform;
            quest3 = nullLoc.transform;
            quest4 = nullLoc.transform;
            quest5 = nullLoc.transform;
        }

    }
    void Update()
    {



        if (locator.enabled == true)
        {
            float distance = Vector3.Distance(location.position, transform.position);

            if (distance >= 400)
            {
                locator.color = new Color(0.16f, 0.40f, 0, 1);
            }
            else if (distance >= 300)
            {
                locator.color = new Color(0.16f, 0.60f, 0, 1);
            }
            else if (distance >= 100)
            {
                locator.color = new Color(0.16f, 0.80f, 0, .8f);
            }
            else if (distance >= 0)
            {
                locator.color = new Color(0.16f, 1f, 0, .6f);
            }
        }


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

    public void SetWayPoint(Transform locationObj)
    {
        locator.enabled = true;
        location = locationObj;

    }
    public void TurnOffQuest()
    {
        locator.enabled = false;
    }
}
