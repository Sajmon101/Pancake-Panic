using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pancake : KitchenObject
{
    private List<KitchenObject> ingredients = new List<KitchenObject>();
    [SerializeField] private KitchenObject tray;
    private float currentOffset;

    private void Start()
    {
        GameObject newIngredient = Instantiate(tray.GetKitchenObjectSO().prefab);
        newIngredient.transform.SetParent(transform);
        newIngredient.transform.localPosition = Vector3.zero;
    }

    public void AddIngredient(KitchenObject ingredient)
    {
        GameObject newIngredient = Instantiate(ingredient.GetKitchenObjectSO().prefab);
        newIngredient.transform.SetParent(transform);
        newIngredient.transform.localPosition = new Vector3(0f, currentOffset + ingredient.GetKitchenObjectSO().height, 0f);

        ingredients.Add(ingredient);

        Player.instance.GetKitchenObject().DestroySelf();
        Player.instance.ClearKitchenObjectParent();
    }

    public List<KitchenObject> GetIngredients()
    {
        return ingredients;
    }
}
