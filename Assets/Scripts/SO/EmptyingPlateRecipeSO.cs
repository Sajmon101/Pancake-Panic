using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EmptyingPlateRecipeSO : ScriptableObject
{
    public KitchenObject fullPlate;
    public KitchenObject halfPlate;
    public KitchenObject emptyPlate;
    public int takesToNextState;
    public KitchenObject kitchenObjectToTake;
}
