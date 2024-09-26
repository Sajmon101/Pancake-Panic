using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Player.Instance.OnCounterSelect += OnTrashSelect;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            player.ClearKitchenObjectParent();
        }
    }

    private void OnTrashSelect(object sender, Player.OnCounterSelectEventArgs e)
    {
        if (e.selectedCounter is TrashCounter)
        {
            animator.SetBool("TrashOpens", true);
        }
        else
        {
            animator.SetBool("TrashOpens", false);
        }
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    animator.SetBool("TrashOpens", true);
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    animator.SetBool("TrashOpens", false);
    //}
}
