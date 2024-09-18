using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseFridgeBtn : MonoBehaviour, IPointerClickHandler
{
    static public EventHandler OnFridgeClose;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnFridgeClose?.Invoke(this, null);
    }
}
