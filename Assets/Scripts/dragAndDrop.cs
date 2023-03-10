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
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        image = gameObject.GetComponent<Image>();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (gameObject.GetComponent<Image>().sprite.name == "None2")
        {
            transform.position = origPosition;
            return;
        }
        else
        {
            for (int i = 0; i < abilityPositions.Count; i++)
            {
                float distanceX = transform.position.x - abilityPositions[i].x;
                float distanceY = abilityPositions[i].y - transform.position.y;

                Sprite temp1 = image.sprite;
                //Debug.Log("x: " + distanceX + ", y: " + distanceY);
                if (distanceX > 290f && distanceY < 40f && distanceY > -40f)
                {
                    Sprite temp2 = gameManager.instance.AbilityFour.GetComponent<Image>().sprite;
                    gameManager.instance.AbilityFour.GetComponent<Image>().sprite = temp1;
                    gameObject.GetComponent<Image>().sprite = temp2;
                    transform.position = origPosition;
                }
                else if (distanceX > 180f && distanceY < 40f && distanceY > -40f)
                {
                    Sprite temp2 = gameManager.instance.AbilityThree.GetComponent<Image>().sprite;
                    gameManager.instance.AbilityThree.GetComponent<Image>().sprite = temp1;
                    gameObject.GetComponent<Image>().sprite = temp2;
                    transform.position = origPosition;
                }
                else if (distanceX > 80f && distanceY < 40f && distanceY > -40f)
                {
                    Sprite temp2 = gameManager.instance.AbilityTwo.GetComponent<Image>().sprite;
                    gameManager.instance.AbilityTwo.GetComponent<Image>().sprite = temp1;
                    gameObject.GetComponent<Image>().sprite = temp2;
                    transform.position = origPosition;
                }
                else if (distanceX > -20f && distanceY < 40f && distanceY > -40f)
                {
                    Sprite temp2 = gameManager.instance.AbilityOne.GetComponent<Image>().sprite;
                    gameManager.instance.AbilityOne.GetComponent<Image>().sprite = temp1;
                    gameObject.GetComponent<Image>().sprite = temp2;
                    transform.position = origPosition;
                }
                else
                {
                    transform.position = origPosition;
                    return;
                }
            }
        }
    }
}
