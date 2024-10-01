using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IngredientPanelManager : MonoBehaviour
{
    Queue<GameObject> ingredientTiles = new();
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject happyFace;
    [SerializeField] GameObject sadFace;
    [SerializeField] OrderCounter orderCounter;

    private void OnEnable()
    {
        sadFace.SetActive(true);
    }

    public void ChangeToHappy()
    {
        happyFace.SetActive(!happyFace.activeSelf);
    }

    public void HideFaces()
    {
        happyFace.SetActive(false);
        sadFace.SetActive(false);
    }

    public void AddIngredientTile(Sprite ingredientSprite)
    {
        GameObject newTile = Instantiate(tilePrefab, transform);
        newTile.GetComponentInChildren<SpriteRenderer>().sprite = ingredientSprite;
        ingredientTiles.Enqueue(newTile);
    }

    public void DeleteHeadIngredientTile()
    {
        Destroy(ingredientTiles.Dequeue());
    }

    

}
