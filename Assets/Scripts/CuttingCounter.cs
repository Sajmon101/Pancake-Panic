using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSO;

    public override void Interact(Player player)
    {
        if (!HaskitchenObject())
        {
            if (player.HaskitchenObject())
            {
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }

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

    public override void InteractAlternate(Player player)
    {
        if (HaskitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO kitchenObjectSO)
    {
        foreach(var cuttingRecipe in cuttingRecipeSO)
        {
            if(cuttingRecipe.input == kitchenObjectSO)
            {
                return cuttingRecipe.output;
            }
        }
        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        foreach (var cuttingRecipe in cuttingRecipeSO)
        {
            if (cuttingRecipe.input == kitchenObjectSO)
            {
                return true;
            }
        }
        return false;
    }

}
