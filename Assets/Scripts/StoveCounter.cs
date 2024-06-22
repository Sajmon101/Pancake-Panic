using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeArrSO;
    [SerializeField] private BurningRecipeSO[] burningRecipeArrSO;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private float fryingTimer = 0;
    private float burningTimer = 0;
    private State state;

    private enum State
    {
        Idle,
        Frying,
        Fried,
        Burnt,
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
            break;

            case State.Frying:

                fryingTimer += Time.deltaTime;
                Debug.Log("frying");
                if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                {

                    fryingTimer = 0;
                    state = State.Fried;

                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                    burningRecipeSO = GetBurningRecipeByInputSO(GetKitchenObject().GetKitchenObjectSO());
                }

            break;

            case State.Fried:

                burningTimer += Time.deltaTime;
                Debug.Log("fried");

                if (burningTimer > burningRecipeSO.burningTimerMax)
                {

                    burningTimer = 0;
                    state = State.Burnt;

                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                }

                break;

            case State.Burnt:
                Debug.Log("burnt");
            break;
        }
    }

    public override void Interact(Player player)
    {
        if (!HaskitchenObject())
        {
            if (player.HaskitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeSO = GetFryingRecipeByInputSO(GetKitchenObject().GetKitchenObjectSO());
                    state = State.Frying;
                    fryingTimer = 0;

                }

            }
        }
        else
        {
            if (!player.HaskitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO kitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeByInputSO(kitchenObjectSO);

        if (fryingRecipeSO != null)
            return fryingRecipeSO.output;
        else
            return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeByInputSO(kitchenObjectSO);

        if (fryingRecipeSO != null)
            return true;
        else
            return false;
    }

    private FryingRecipeSO GetFryingRecipeByInputSO(KitchenObjectSO kitchenObjectSO)
    {
        foreach (var fryingRecipe in fryingRecipeArrSO)
        {
            if (fryingRecipe.input == kitchenObjectSO)
            {
                return fryingRecipe;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeByInputSO(KitchenObjectSO kitchenObjectSO)
    {
        foreach (var burningRecipe in burningRecipeArrSO)
        {
            if (burningRecipe.input == kitchenObjectSO)
            {
                return burningRecipe;
            }
        }
        return null;
    }
}
