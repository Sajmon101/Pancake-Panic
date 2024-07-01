using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Counter : BaseCounter
{

    KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        
        if(!HaskitchenObject())
        {
            //Counter is free
            if (player.HaskitchenObject())
            {
                //Counter is free and player holds sth
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            //There is sth on counter
            if (!player.HaskitchenObject())
            {
                //There is sth on the counter and player doesn't hold anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            else
            {
                //There is sth on the counter and player holds sth
                if(player.GetKitchenObject() is PlateKitchenObject)
                {
                    //There is sth on the counter and player holds plate
                    PlateKitchenObject plate = player.GetKitchenObject() as PlateKitchenObject;
                    var a = plate.TryAddIngredient(kitchenObjectSO);
                    Debug.Log(kitchenObjectSO);
                    if (a)
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
        }
    }
}
