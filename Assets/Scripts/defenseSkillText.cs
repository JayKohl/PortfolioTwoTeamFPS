using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class defenseSkillText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject defenseskilltext;
    void Start()
    {
        defenseskilltext.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        defenseskilltext.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        defenseskilltext.SetActive(false);
    }
}
