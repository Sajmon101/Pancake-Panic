using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter
{
    public EventHandler<OnStateChanheEventArgs> OnStateChange;

    public class OnStateChanheEventArgs
    {
        public State state;
    }


    [SerializeField] private FryingRecipeSO[] fryingRecipeArrSO;
    [SerializeField] private BurningRecipeSO[] burningRecipeArrSO;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private float fryingTimer = 0;
    private float burningTimer = 0;
    private State state;

    public enum State
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

                if (burningTimer > burningRecipeSO.burningTimerMax)
                {

                    burningTimer = 0;
                    state = State.Burnt;

                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                    OnStateChange?.Invoke(this, new OnStateChanheEventArgs
                    {
                        state = state,
                    });
                }

                break;

            case State.Burnt:
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

                    OnStateChange?.Invoke(this, new OnStateChanheEventArgs
                    {
                        state = state,
                    });

                }

            }
        }
        else
        {
            if (!player.HaskitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;

                OnStateChange?.Invoke(this, new OnStateChanheEventArgs
                {
                    state = state,
                });
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
