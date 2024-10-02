using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderGenerator : MonoBehaviour
{
    [SerializeField] private float orderSpawnInterval;
    [SerializeField] int ordersOnStart;
    [SerializeField] int ordersToGenerate;
    [SerializeField] int maxQueueSizeAllowed;
    [SerializeField] KitchenObjectSO[] avaliableIngredientsSO;
    private Queue<Queue<KitchenObjectSO>> ordersQueue = new();
    [SerializeField] private List<OrderCounter> orderCounters = new();
    [SerializeField] private TextMeshProUGUI OrdersLeftUI;
    [SerializeField] private TextMeshProUGUI WaitingUI;
    public event System.EventHandler OnGameWin;
    public event System.EventHandler OnGameLose;

    private void OnEnable()
    {
        foreach (OrderCounter orderCounter in orderCounters)
        {
            orderCounter.OnOrderComplete += CheckIfWin;
        }
    }

    private void Start()
    {

        for (int i = 0; i < ordersOnStart; i++)
        {
            ordersQueue.Enqueue(GenerateOrder());
            ordersToGenerate--;
        }

        StartCoroutine(GeneratingOrdersCoroutine());

        UpdateScreenUI();
    }

    private void Update()
    {
        UpdateScreenUI();
        GiveOrdersToCounters();
    }

    private IEnumerator GeneratingOrdersCoroutine()
    {
        while (ordersToGenerate > 0)
        {
            yield return new WaitForSeconds(orderSpawnInterval);
            ordersQueue.Enqueue(GenerateOrder());
            ordersToGenerate--;

            if(ordersQueue.Count > maxQueueSizeAllowed)
            {
                OnGameLose?.Invoke(this, null);
            }

            UpdateScreenUI();
        }
    }

    private Queue<KitchenObjectSO> GenerateOrder()
    {
        Queue<KitchenObjectSO> drawnIngredientsSO = new();

        int ingredientsAmount = Random.Range(1, 5);
        drawnIngredientsSO.Enqueue(avaliableIngredientsSO[0]);

        for (int i = 0; i < ingredientsAmount; i++)
        {
            int ingredientIndex = Random.Range(0, avaliableIngredientsSO.Length);
            drawnIngredientsSO.Enqueue(avaliableIngredientsSO[ingredientIndex]);
        }

        return drawnIngredientsSO;
    }

    private void GiveOrdersToCounters()
    {
        if (ordersQueue.Count > 0)
        {
            foreach (OrderCounter orderCounter in orderCounters)
            {
                if (!orderCounter.HasKitchenObject())
                {
                    orderCounter.PlaceOrder(ordersQueue.Dequeue());
                    UpdateScreenUI();
                    return;
                }
            }
        }
    }

    private void UpdateScreenUI()
    {
        OrdersLeftUI.text = ordersToGenerate.ToString();
        WaitingUI.text = ordersQueue.Count.ToString() + "/" + maxQueueSizeAllowed;
    }

    private void CheckIfWin(object sender, System.EventArgs e)
    {
        if(ordersQueue.Count == 0)
        {
            foreach (OrderCounter orderCounter in orderCounters)
            {
                if(orderCounter.HasKitchenObject())
                {
                    return;
                }
            }

            OnGameWin?.Invoke(this, null);
            
        }
    }
}
