using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderCounter : BaseCounter
{
    private float orderSpawnInterval = 15f;
    private float timer = 0f;
    [SerializeField] bool generateOrderOnStart;
    [SerializeField] int ordersToGenerate;
    [SerializeField] KitchenObjectSO[] avaliableIngredientsSO;
    public KitchenObjectSO emptyPancakeSO;
    public GameObject ingredientPanel;
    public GameObject itemPrefab;
    Queue<List<KitchenObjectSO>> ordersQueue = new();

    private void Start()
    {
        if (generateOrderOnStart)
        {
            GenerateOrder();
            CreateUI();
        }

        StartCoroutine(GeneratingOrdersCoroutine());
    }

    private IEnumerator GeneratingOrdersCoroutine()
    {
        for (int i = 0; i < ordersToGenerate; i++)
        {
            GenerateOrder();
            yield return new WaitForSeconds(orderSpawnInterval);
        }
    }

    private List<KitchenObjectSO> GenerateOrder()
    {
        List<KitchenObjectSO> drawnIngredientsSO = new();
        KitchenObject.SpawnKitchenObject(emptyPancakeSO, this);

        int ingredientsAmount = Random.Range(2, 6);
        drawnIngredientsSO.Add(avaliableIngredientsSO[0]);

        for (int i = 0; i < ingredientsAmount - 1; i++)
        {
            int ingredientIndex = Random.Range(0, avaliableIngredientsSO.Length);
            drawnIngredientsSO.Add(avaliableIngredientsSO[i]);
        }

        CreateUI(); // to wyrzuciæ do miejsca gdzie bêdzie pobierane zamówienie z kolejki

        return drawnIngredientsSO;
    }

    private void CreateUI()
    {
        ingredientPanel.SetActive(true);
        foreach(var ingredientSO in drawnIngredientsSO)
        {
            ingredientPanel.GetComponent<IngredientPanelManager>().AddIngredientTile(ingredientSO.sprite);
        }
    }

    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            GetKitchenObject().GetComponent<Pancake>().AddIngredient(player.GetKitchenObject());
        }
    }

}
