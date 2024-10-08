using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderCounter : BaseCounter
{
    public KitchenObjectSO emptyPancakeSO;
    public GameObject ingredientPanel;
    public GameObject itemPrefab;
    private Queue<KitchenObjectSO> currentIngredientQueue = new();
    public event EventHandler OnOrderComplete;
    private AudioSource orderCompleteSound;
    //private bool orderFinished = false;

    private void Awake()
    {
        orderCompleteSound = GetComponent<AudioSource>();
    }

    public void PlaceOrder(Queue<KitchenObjectSO> ingredientQueue)
    {
        currentIngredientQueue = ingredientQueue;
        KitchenObject.SpawnKitchenObject(emptyPancakeSO, this);
        CreateUI(currentIngredientQueue);
    }

    private void CreateUI(Queue<KitchenObjectSO> ingredientQueue)
    {
        ingredientPanel.SetActive(true);
        foreach (var ingredientSO in ingredientQueue)
        {
            ingredientPanel.GetComponent<IngredientPanelManager>().AddIngredientTile(ingredientSO.sprite);
        }
    }

    public override void Interact(Player player)
    {
        //uncomment to player be able to take ready pancake
        /*if (orderFinished && !player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);
        }
        else */ if (HasKitchenObject() && player.HasKitchenObject())
        {
            if (GetKitchenObject().GetComponent<Pancake>().AreMatching(player.GetKitchenObject().GetKitchenObjectSO(), currentIngredientQueue.Peek()))
            {
                GetKitchenObject().GetComponent<Pancake>().AddIngredient(player.GetKitchenObject());
                ingredientPanel.GetComponent<IngredientPanelManager>().DeleteHeadIngredientTile();
                currentIngredientQueue.Dequeue();

                if (currentIngredientQueue.Count == 0)
                    EndCurrentOrder();
            }
        }

    }

    private void EndCurrentOrder()
    {
        ingredientPanel.SetActive(false);
        currentIngredientQueue = null;
        orderCompleteSound.Play();
        //orderFinished = true;
        //GetKitchenObject().GetComponent<Pancake>().AnimateAndDestroy();
        ingredientPanel.GetComponent <IngredientPanelManager>().ChangeToHappy();
        StartCoroutine(DestroyPancake());
    } 

    private IEnumerator DestroyPancake()
    {
        yield return new WaitForSeconds(1.5f);
        ingredientPanel.GetComponent<IngredientPanelManager>().HideFaces();
        GetKitchenObject().GetComponent<Pancake>().DestroySelf();
        ClearKitchenObjectParent();
        OnOrderComplete?.Invoke(this, null);
    }
}
