using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class damageSkillText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject damageskilltext;
    void Start()
    {
        damageskilltext.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        damageskilltext.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        damageskilltext.SetActive(false);
    }
}
