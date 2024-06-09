using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent
{
    void SetKitchenObject(KitchenObject kitchenObjectParent);

    Transform GetTopPoint();

    void ClearKitchenObjectParent();

    IKitchenObjectParent GetKitchenObjectParent();

    bool HaskitchenObjectParent();
}
