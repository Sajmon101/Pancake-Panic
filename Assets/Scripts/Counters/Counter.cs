using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Counter : BaseCounter
{

    [SerializeField] KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if(!HaskitchenObject())
        {
            if(player.HaskitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            if (!player.HaskitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
