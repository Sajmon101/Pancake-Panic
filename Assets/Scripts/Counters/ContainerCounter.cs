using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] GameObject ChooseKitchenObjectUI;
    public static EventHandler OnFridgeInteraction;
    private Animator fridgeAnimator;

    private void Awake()
    {
        FridgeButtonManager.OnFridgeButtonClick += SpawnObjectToPlayer;
        CloseFridgeBtn.OnFridgeClose += OnCloseFridge;
        fridgeAnimator = GetComponent<Animator>();
    }

    public override void Interact(Player player)
    {
        OpenFridge();

        OnFridgeInteraction?.Invoke(this, null);
    }

    private void OpenFridge()
    {
        fridgeAnimator.SetBool("fridgeOpens", true);
        Player.instance.DisablePlayerMovement();
        ChooseKitchenObjectUI.SetActive(true);
    }

    private void OnCloseFridge(object sender, EventArgs e)
    {
        CloseFridge();
    }

    private void CloseFridge()
    {
        fridgeAnimator.SetBool("fridgeOpens", false);
        Player.instance.EnablePlayerMovement();
        ChooseKitchenObjectUI.SetActive(false);
    }

    private void SpawnObjectToPlayer(object sender, KitchenObjectSO kitchenObjectSO)
    {
        if (!Player.instance.HaskitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, Player.instance);
        }

        CloseFridge();
    }

    private void OnDisable()
    {
        FridgeButtonManager.OnFridgeButtonClick -= SpawnObjectToPlayer;
        CloseFridgeBtn.OnFridgeClose -= OnCloseFridge;
    }
}
