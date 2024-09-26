using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField] BaseCounter counter;
    [SerializeField] GameObject[] highlightedVisualObject;

    private void Start()
    {
        Player.Instance.OnCounterSelect += Player_OnCounterSelect;
    }

    private void Player_OnCounterSelect(object sender, Player.OnCounterSelectEventArgs e)
    {
        if(e.selectedCounter == counter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (var item in highlightedVisualObject)
        {
            if (!item.activeSelf)
            {
                item.SetActive(true);
            }
        }
    }

    private void Hide()
    {
        foreach (var item in highlightedVisualObject)
        {
            if (item.activeSelf)
            {
                item.SetActive(false);
            }
        }
    }
}
