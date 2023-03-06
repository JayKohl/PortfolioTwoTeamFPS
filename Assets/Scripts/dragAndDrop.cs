using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class dragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Vector3 origPosition;
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    [SerializeField] GameObject ability1;
    [SerializeField] GameObject ability2;
    [SerializeField] GameObject ability3;
    [SerializeField] GameObject ability4;
    List<Vector3> abilityPositions;

    private void Start()
    {
        origPosition = transform.position;
        abilityPositions =  new List<Vector3>();
        abilityPositions.Add(ability1.transform.position);
        abilityPositions.Add(ability2.transform.position);
        abilityPositions.Add(ability3.transform.position);
        abilityPositions.Add(ability4.transform.position);
        image = gameObject.GetComponent<Image>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //parentAfterDrag = transform.parent;
        //transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {        
        for (int i = 0; i < abilityPositions.Count; i++)
        {
            Vector3 distance = abilityPositions[i] - transform.position;

            if (distance.x < 10f && distance.y < 10f)
            {
                Debug.Log("ability position: " + i);
            }
        }
        image.raycastTarget = true;
        transform.position = origPosition;
    }
}
