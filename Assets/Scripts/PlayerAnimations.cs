using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator animController;
    private bool isWalking = false;

    private void Awake()
    {
        animController = GetComponent<Animator>();   
    }

    void Start()
    {
        ContainerCounter.OnFridgeInteraction += OnFridgeOpen;
        Player.Instance.OnHoldObject += OnHoldingObject;
        ScheduleNextAction();
    }
    private void Update()
    {
        bool playerIsWalking = Player.Instance.IsWalking();

        if (playerIsWalking != isWalking)
        {
            if (playerIsWalking)
            {
                animController.SetFloat("speed", 1f);
            }
            else
            {
                animController.SetFloat("speed", 0f);
            }

            isWalking = playerIsWalking;
        }
    }

    private void OnHoldingObject(object sender, bool isHolding)
    {
        animController.SetBool("isHolding", isHolding);
    }

    private void OnFridgeOpen(object sender, EventArgs e)
    {
        animController.SetBool("openFridgeTrigger", true);
    }

    void ScheduleNextAction()
    {
        // Losowo wybiera czas (np. od 5 do 15 sekund) przed kolejnym losowaniem animacji
        float delay = 10f;
        Invoke("TriggerRandomAction", delay);
    }

    void TriggerRandomAction()
    {
        int randomNumber = UnityEngine.Random.Range(0, 2);
        bool needDance = randomNumber == 0;
        animController.SetBool("danceOrLook", needDance);
        ScheduleNextAction();
    }
}
