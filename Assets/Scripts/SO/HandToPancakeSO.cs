using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class HandToPancakeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public bool destoryInput;
}
