using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Counter : MonoBehaviour
{

    [SerializeField] KitchenObjectSO kitchenObjectSO;
    [SerializeField] Transform CounterTopPoint;

    public void Interact()
    {
        Debug.Log("Interact");
        Transform kitchenObjectSOTransform = Instantiate(kitchenObjectSO.prefab.transform, CounterTopPoint);
        kitchenObjectSOTransform.localPosition = Vector3.zero;
        Debug.Log(kitchenObjectSOTransform.GetComponent<KitchenObject>().GetKitchenObjectSO().objectName);

    }
}
