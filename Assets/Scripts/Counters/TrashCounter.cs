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

    public override void Interact(Player player)
    {
        if (player.HaskitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            player.ClearKitchenObjectParent();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        animator.SetBool("TrashOpens", true);
    }

    void OnTriggerExit(Collider other)
    {
        animator.SetBool("TrashOpens", false);
    }
}
