using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [HideInInspector]
    public bool IsPressed;

    public void OnPointerUp(PointerEventData eventData)
    {
        IsPressed = false;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        IsPressed = true;
    }
}
