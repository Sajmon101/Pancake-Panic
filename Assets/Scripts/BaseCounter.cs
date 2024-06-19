using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{

    [SerializeField] Transform CounterTopPoint;

    private KitchenObject kitchenObject;

    public virtual void Interact(Player player)
    {
        Debug.LogError("Missing override from BaseCounter");
    }

    public void SetKitchenObjectParent(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public Transform GetTopPoint()
    {
        return CounterTopPoint;
    }

    public void ClearKitchenObjectParent()
    {
        kitchenObject = null;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public bool HaskitchenObject()
    {
        return kitchenObject != null;
    }
}
