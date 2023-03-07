using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class abilitySkillText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject abilityskilltext;
    void Start()
    {
        abilityskilltext.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        abilityskilltext.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        abilityskilltext.SetActive(false);
    }
}
