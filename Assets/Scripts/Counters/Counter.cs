using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Counter : BaseCounter
{
    [SerializeField] bool hasStartObject;
    [SerializeField] KitchenObjectSO startObject;
    [SerializeField] EmptyingPlateRecipeSO[] emptyingPlateRecipeArrSO;
    private int currentTakes = 0;

    private void Start()
    {
        if (hasStartObject && startObject != null)
        {
            KitchenObject.SpawnKitchenObject(startObject, this);
        }
    }

    public override void Interact(Player player)
    {
        
        if(!HasKitchenObject())
        {
            //Counter is free
            if (player.HasKitchenObject())
            {
                //Counter is free and player holds sth
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            //There is sth on counter
            if (!player.HasKitchenObject())
            {
                //player does not hold anything and there is multiple take object on counter
                EmptyingPlateRecipeSO emptyingPlateRecipeSO = GetEmptyingRecipeByInputSO(GetKitchenObject().GetKitchenObjectSO());

                if (emptyingPlateRecipeSO)
                {
                    HandleMultipleTakeObjects(emptyingPlateRecipeSO);
                }

                else
                {
                    //There is sth on the counter and player doesn't hold anything
                    GetKitchenObject().SetKitchenObjectParent(player);
                }
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

    private EmptyingPlateRecipeSO GetEmptyingRecipeByInputSO(KitchenObjectSO kitchenObjectSO)
    {
        foreach (var emptyingRecipe in emptyingPlateRecipeArrSO)
        {
            if (emptyingRecipe.fullPlate.GetKitchenObjectSO() == kitchenObjectSO || emptyingRecipe.halfPlate.GetKitchenObjectSO() == kitchenObjectSO)
            {
                
                return emptyingRecipe;
            }
        }
        return null;
    }

    enum PlateStates
    {
        FullPlate,
        HalfPlate,
        EmptyPlate
    }

    private PlateStates currentState;

    private void HandleMultipleTakeObjects(EmptyingPlateRecipeSO emptyingRecipe)
    {
        switch (currentState)
        {
            case PlateStates.FullPlate:

                KitchenObject kitchenObjectOutput = Instantiate(emptyingRecipe.kitchenObjectToTake);
                kitchenObjectOutput.transform.localPosition = Vector3.zero;
                kitchenObjectOutput.SetKitchenObjectParent(Player.Instance);
                currentTakes++;

                if (currentTakes == emptyingRecipe.takesToNextState)
                {
                    GoToState(PlateStates.HalfPlate, emptyingRecipe);
                }

            break;

            case PlateStates.HalfPlate:

                kitchenObjectOutput = Instantiate(emptyingRecipe.kitchenObjectToTake);
                kitchenObjectOutput.transform.localPosition = Vector3.zero;
                kitchenObjectOutput.SetKitchenObjectParent(Player.Instance);
                currentTakes++;

                if (currentTakes == emptyingRecipe.takesToNextState)
                {
                    GoToState(PlateStates.EmptyPlate, emptyingRecipe);
                }

            break;

            case PlateStates.EmptyPlate:

                GetKitchenObject().SetKitchenObjectParent(Player.Instance);

            break;
        }
    }

    private void GoToState(PlateStates state, EmptyingPlateRecipeSO emptyingRecipe)
    {
        currentTakes = 0;
        currentState = state;
        GetKitchenObject().DestroySelf();

        switch (currentState)
        {
            case PlateStates.HalfPlate:
                KitchenObject halfPlate = Instantiate(emptyingRecipe.halfPlate);
                halfPlate.transform.localPosition = Vector3.zero;
                halfPlate.SetKitchenObjectParent(this);
                break;

            case PlateStates.EmptyPlate:
                KitchenObject emptyPlate = Instantiate(emptyingRecipe.emptyPlate);
                emptyPlate.transform.localPosition = Vector3.zero;
                emptyPlate.SetKitchenObjectParent(this);
                break;
        }
    }

}
