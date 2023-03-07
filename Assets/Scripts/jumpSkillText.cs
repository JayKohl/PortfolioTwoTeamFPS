using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class jumpSkillText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject jumpskilltext;
    void Start()
    {
        jumpskilltext.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        jumpskilltext.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        jumpskilltext.SetActive(false);
    }
}
