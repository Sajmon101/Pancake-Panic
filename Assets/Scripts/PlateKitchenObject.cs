using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{

    [SerializeField] private List<KitchenObjectSO> validIngredients;
    List<KitchenObjectSO> kitchenObjects;

    private void Awake()
    {
        kitchenObjects = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if(validIngredients.Contains(kitchenObjectSO))
        {
            kitchenObjects.Add(kitchenObjectSO);
            return true;
        }
        else
        {
            return false;
        }

    }
}
