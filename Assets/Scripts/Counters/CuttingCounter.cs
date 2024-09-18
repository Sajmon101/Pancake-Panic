using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;


    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSO;
    [SerializeField] private GameObject knife;
    private float cuttingProgress;
    private float cuttingInterval = 0.1f;
    private float cuttingProgressIncrement = 0.2f;

    public override void Interact(Player player)
    {
        if (!HaskitchenObject())
        {
            if (player.HaskitchenObject())
            {
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    StartCoroutine(FillBarCoroutine());
                    knife.SetActive(true);
                }

            }
        }
        else
        {
            if (!player.HaskitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            else
            {
                if (player.GetKitchenObject() is PlateKitchenObject)
                {
                    //There is sth on the counter and player holds plate
                    PlateKitchenObject plate = player.GetKitchenObject() as PlateKitchenObject;

                    if (plate.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
        }
    }

    private IEnumerator FillBarCoroutine()
    {
        cuttingProgress = 0;

        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(GetKitchenObject().GetKitchenObjectSO());

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = cuttingProgress / cuttingRecipeSO.cuttingProgressMax
        });

        while (cuttingProgress < cuttingRecipeSO.cuttingProgressMax)
        {
            yield return new WaitForSeconds(cuttingInterval);
            cuttingProgress += cuttingProgressIncrement;

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
        }

        KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
        GetKitchenObject().DestroySelf();
        KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
        knife.SetActive(false);
    }

    //public override void InteractAlternate(Player player)
    //{
    //    if (HaskitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
    //    {
    //        cuttingProgress++;

    //        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(GetKitchenObject().GetKitchenObjectSO());

    //        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
    //        {
    //            progressNormalized = (float)cuttingProgress/cuttingRecipeSO.cuttingProgressMax
    //        });

    //        if (cuttingRecipeSO.cuttingProgressMax <= cuttingProgress)
    //        {
    //            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
    //            GetKitchenObject().DestroySelf();
    //            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
    //        }
    //    }
    //}

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
