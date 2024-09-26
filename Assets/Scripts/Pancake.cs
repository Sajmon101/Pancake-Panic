using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pancake : KitchenObject
{
    private List<KitchenObjectSO> ingredients = new List<KitchenObjectSO>();
    [SerializeField] private HandToPancakeSO[] handToPancakeRecipesArr;
    [SerializeField] private KitchenObject trayKitchenObject;
    private float offsetSum = 0f;
    [SerializeField] private GameObject blowPSPrefab;

    private void Start()
    {
        GameObject newIngredient = Instantiate(trayKitchenObject.GetKitchenObjectSO().prefab);
        newIngredient.transform.SetParent(transform);
        newIngredient.transform.localPosition = Vector3.zero;
        offsetSum += trayKitchenObject.GetKitchenObjectSO().heightOnPancake;
    }

    public void AddIngredient(KitchenObject ingredient)
    {
        HandToPancakeSO reciepe = GetHandToPancakeRecipeSO(ingredient.GetKitchenObjectSO());

        KitchenObjectSO tranformedIngredient = reciepe?.output;

        if (tranformedIngredient != null)
        {
            GameObject newIngredient = Instantiate(tranformedIngredient.prefab);
            newIngredient.transform.SetParent(transform);

            newIngredient.transform.localPosition = new Vector3(0f, offsetSum, 0f);
            offsetSum += tranformedIngredient.heightOnPancake;

            ingredients.Add(tranformedIngredient);

            if(reciepe.destoryInput)
            {
                Player.Instance.GetKitchenObject().DestroySelf();
                Player.Instance.ClearKitchenObjectParent();
            }
        }
    }

    private HandToPancakeSO GetHandToPancakeRecipeSO(KitchenObjectSO kitchenObjectSO)
    {
        foreach (var handToPancakeRecipe in handToPancakeRecipesArr)
        {
            if (handToPancakeRecipe.input == kitchenObjectSO)
            {
                return handToPancakeRecipe;
            }
        }
        return null;
    }

    public bool AreMatching(KitchenObjectSO input, KitchenObjectSO output)
    {
        HandToPancakeSO reciepe = GetHandToPancakeRecipeSO(input);
        KitchenObjectSO tranformedIngredient = reciepe.output;

        if(tranformedIngredient.name == output.name)
            return true;
        else
            return false;

    }

    public void AnimateAndDestroy()
    {
        GetComponent<Animator>().SetBool("destroy", true);
    }

    public void OnAnimationEnd()
    {
        Instantiate(blowPSPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
