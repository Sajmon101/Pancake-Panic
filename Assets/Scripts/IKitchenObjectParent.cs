using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent
{
    void SetKitchenObjectParent(KitchenObject kitchenObject);

    Transform GetTopPoint();

    void ClearKitchenObjectParent();
    KitchenObject GetKitchenObject();

    bool HaskitchenObject();
}
