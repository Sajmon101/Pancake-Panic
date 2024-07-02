using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    public EventHandler<OnStateChangeEventArgs> OnStateChange;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangeEventArgs
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

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = (float)fryingTimer/fryingRecipeSO.fryingTimerMax
                });

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

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = (float)burningTimer/burningRecipeSO.burningTimerMax
                });

                if (burningTimer > burningRecipeSO.burningTimerMax)
                {

                    burningTimer = 0;
                    state = State.Burnt;

                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                    OnStateChange?.Invoke(this, new OnStateChangeEventArgs
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

                    OnStateChange?.Invoke(this, new OnStateChangeEventArgs
                    {
                        state = state,
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = 0f
                    });

                }

            }
        }
        else
        {
            if (!player.HaskitchenObject())
            {
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });

                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;

                OnStateChange?.Invoke(this, new OnStateChangeEventArgs
                {
                    state = state,
                });


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

                    state = State.Idle;

                    OnStateChange?.Invoke(this, new OnStateChangeEventArgs
                    {
                        state = state,
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = 0f
                    });
                }
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
