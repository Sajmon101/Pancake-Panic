using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObjectParent();
        }

        this.kitchenObjectParent = kitchenObjectParent;

        this.kitchenObjectParent.SetKitchenObjectParent(this);

        transform.parent  = kitchenObjectParent.GetTopPoint();
        transform.localPosition = Vector3.zero;
    }

    public Transform GetTopPoint()
    {
        return null;
    }

    public void ClearKitchenObjectParent()
    {

    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return null;
    }

    public bool HaskitchenObjectParent()
    {
        return false;
    }

}
