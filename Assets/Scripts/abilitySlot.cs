using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class abilitySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Debug.Log("dropped: "+dropped);
        gameObject.GetComponent<Image>().sprite = dropped.GetComponent<Image>().sprite;
    }
}
