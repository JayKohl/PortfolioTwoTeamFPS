using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class speedSkillText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject speedskilltext;
    void Start()
    {
        speedskilltext.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        speedskilltext.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        speedskilltext.SetActive(false);
    }
}
