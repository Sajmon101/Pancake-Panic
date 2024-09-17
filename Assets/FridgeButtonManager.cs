using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FridgeButtonManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] KitchenObjectSO kitchenObjectSO;
    static public event EventHandler<KitchenObjectSO> OnFridgeButtonClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnFridgeButtonClick?.Invoke(this, kitchenObjectSO);
    }
}
