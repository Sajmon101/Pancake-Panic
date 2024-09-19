using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;


    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }


    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObjectParent();
        }

        this.kitchenObjectParent = kitchenObjectParent;

        this.kitchenObjectParent.SetKitchenObjectParent(this);

        transform.parent  = kitchenObjectParent.GetTopPoint();
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        //kitchenObjectParent.kitchen
    }

    public Transform GetTopPoint()
    {
        return null;
    }

    public void ClearKitchenObjectParent()
    {

        kitchenObjectParent = null;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return null;
    }

    public bool HaskitchenObjectParent()
    {
        return false;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }    

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectSOTransform = Instantiate(kitchenObjectSO.prefab.transform);
        KitchenObject kitchenObject = kitchenObjectSOTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }

}
