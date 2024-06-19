using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] KitchenObjectSO kitchenObjectSO;

    KitchenObject kitchenObject;

    public override void Interact(Player player)
    {
        if(!player.HaskitchenObject())
        {
            Transform kitchenObjectSOTransform = Instantiate(kitchenObjectSO.prefab.transform);
            kitchenObjectSOTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
        }
    }
}
