using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public bool isHolding{get;private set;}

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding=true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding=false;
    }

    private void Awake()
    {
        isHolding=false;
    }

}
