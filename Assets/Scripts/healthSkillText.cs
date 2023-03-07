using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class healthSkillText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject healthskilltext;

    void Start()
    {
        healthskilltext.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        healthskilltext.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        healthskilltext.SetActive(false);
    }
    

    //public void OnMouseOver()
    //{
    //    healthskilltext.SetActive(true);
    //}

    //public void OnMouseExit()
    //{
    //    healthskilltext.SetActive(false);
    //}
}
