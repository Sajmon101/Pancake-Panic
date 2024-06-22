using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StoveCounter;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] GameObject fryingPS;
    [SerializeField] StoveCounter StoveCounter;

    private void Start()
    {
        StoveCounter.OnStateChange += StoveCounter_OnStateChange;
    }

    private void StoveCounter_OnStateChange(object sender, StoveCounter.OnStateChanheEventArgs e)
    {
        if (e.state == State.Frying)
        {
            fryingPS.SetActive(true);
        }
        else
        {
            fryingPS.SetActive(false);
        }
    }
}
