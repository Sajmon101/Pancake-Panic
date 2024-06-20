using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSO;
    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HaskitchenObject())
        {
            if (player.HaskitchenObject())
            {
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
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
            cuttingProgress++;
            if(GetCuttingRecipeSO(GetKitchenObject().GetKitchenObjectSO()).cuttingProgressMax <= cuttingProgress)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO kitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(kitchenObjectSO);

        if (cuttingRecipeSO != null)
            return cuttingRecipeSO.output;
        else
            return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(kitchenObjectSO);

        if (cuttingRecipeSO != null)
            return true;
        else
            return false;
    }

    private CuttingRecipeSO GetCuttingRecipeSO(KitchenObjectSO kitchenObjectSO)
    {
        foreach (var cuttingRecipe in cuttingRecipeSO)
        {
            if (cuttingRecipe.input == kitchenObjectSO)
            {
                return cuttingRecipe;
            }
        }
        return null;
    }

}
