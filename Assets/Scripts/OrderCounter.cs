using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderCounter : BaseCounter
{
    private float orderSpawnInterval = 15f;  // Interwa³ czasu generowania zamówieñ
    private float timer;
    [SerializeField] bool generateOnAwake;
    [SerializeField] KitchenObjectSO[] avaliableIngredients;
    private List<KitchenObjectSO> drawnIngredients = new();
    public KitchenObjectSO emptyPancakeSO;
    public GameObject panel;
    public GameObject itemPrefab;

    private void Start()
    {
        if (generateOnAwake)
        {
            timer = 15.1f;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= orderSpawnInterval)
        {
            GenerateOrder();
            CreateUI();
            timer = 0;
        }
    }

    private void GenerateOrder()
    {
        if(!HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(emptyPancakeSO, this);
            //GetKitchenObject().GetComponent<Pancake>().AddIngredient(trayPrefab);

            int ingredientsAmount = Random.Range(2, 6);
            drawnIngredients.Add(avaliableIngredients[0]);

            for (int i = 0; i < ingredientsAmount - 1; i++)
            {
                int ingredientIndex = Random.Range(0, avaliableIngredients.Length);
                drawnIngredients.Add(avaliableIngredients[i]);
            }
        }
    }

    private void CreateUI()
    {
        // Tworzy nowy element w panelu
        GameObject newItem = Instantiate(itemPrefab, panel.transform);

        // Mo¿esz ustawiæ treœæ nowego elementu np. tekst zamówienia
        Text newItemText = newItem.GetComponentInChildren<Text>();
        if (newItemText != null)
        {
            newItemText.text = "Nowe zamówienie " + Random.Range(1, 100).ToString();
        }
    }

    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            GetKitchenObject().GetComponent<Pancake>().AddIngredient(player.GetKitchenObject());
            //KitchenObject heldByPlayer;
            //heldByPlayer = player.GetKitchenObject();
            //heldByPlayer.transform.SetParent(GetKitchenObject().transform);
            //heldByPlayer.transform.localPosition = Vector3.zero;
            //player.ClearKitchenObjectParent();
        }
    }

}
