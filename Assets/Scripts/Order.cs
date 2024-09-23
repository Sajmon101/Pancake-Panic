using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public List<KitchenObjectSO> Toppings { get; set; }

    public Order(List<KitchenObjectSO> toppings)
    {
        Toppings = toppings;
    }
}
