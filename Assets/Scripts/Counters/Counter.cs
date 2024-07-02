using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Counter : BaseCounter
{

    //KitchenObjectSO kitchenObjectSO;

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

                    if (plate.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    //There is sth on the counter and player holds sth different from plate
                    if (GetKitchenObject() is PlateKitchenObject)
                    {
                        //There is plate on the counter and player holds sth different from plate
                        PlateKitchenObject plate = GetKitchenObject() as PlateKitchenObject;

                        if (plate.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
        }
    }
}
