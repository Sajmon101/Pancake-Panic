using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Counter : MonoBehaviour, IKitchenObjectParent
{

    [SerializeField] KitchenObjectSO kitchenObjectSO;
    [SerializeField] Transform CounterTopPoint;

    private IKitchenObjectParent kitchenObjectParent;


    public void Interact()
    {
        if (kitchenObjectParent == null)
        {
            Transform kitchenObjectSOTransform = Instantiate(kitchenObjectSO.prefab.transform, CounterTopPoint);
            kitchenObjectSOTransform.localPosition = Vector3.zero;
            kitchenObjectParent = kitchenObjectSOTransform.GetComponent<IKitchenObjectParent>();
            kitchenObjectParent.SetKitchenObjectParent(this);
        }
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
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
