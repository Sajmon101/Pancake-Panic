using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] KitchenObjectSO kitchenObjectSO;
    public static EventHandler OnFridgeInteraction;

    public override void Interact(Player player)
    {
        if(!player.HaskitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
        }

        OnFridgeInteraction?.Invoke(this, null);
    }
}
