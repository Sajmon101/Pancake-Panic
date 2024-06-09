using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] KitchenObjectSO kitchenObjectSO;
    [SerializeField] Transform CounterTopPoint;

    private KitchenObject kitchenObjectParent;


    public override void Interact(Player player)
    {
        if (kitchenObjectParent == null)
        {
            Transform kitchenObjectSOTransform = Instantiate(kitchenObjectSO.prefab.transform, CounterTopPoint);
            kitchenObjectSOTransform.localPosition = Vector3.zero;
            kitchenObjectParent = kitchenObjectSOTransform.GetComponent<KitchenObject>();
            kitchenObjectParent.SetKitchenObjectParent(this);
        }
        else
        {
            kitchenObjectParent.SetKitchenObjectParent(player);
        }
    }

    public void SetKitchenObject(KitchenObject kitchenObjectParent)
    {
        this.kitchenObjectParent = kitchenObjectParent;
    }

    public Transform GetTopPoint()
    {
        return CounterTopPoint;
    }

    public void ClearKitchenObjectParent()
    {
        kitchenObjectParent = null;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return this;
    }

    public bool HaskitchenObjectParent()
    {
        return kitchenObjectParent != null;
    }
}
