using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class XPSkillText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject XPskilltext;
    void Start()
    {
        XPskilltext.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        XPskilltext.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        XPskilltext.SetActive(false);
    }
}
