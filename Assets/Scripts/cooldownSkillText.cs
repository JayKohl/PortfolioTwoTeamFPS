using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class cooldownSkillText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject cooldownskilltext;
    void Start()
    {
        cooldownskilltext.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cooldownskilltext.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cooldownskilltext.SetActive(false);
    }
}
