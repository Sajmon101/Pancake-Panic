using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent
{
    void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent);

    Transform GetTopPoint();

    void ClearKitchenObjectParent();

    IKitchenObjectParent GetKitchenObjectParent();

    bool HaskitchenObjectParent();
}
