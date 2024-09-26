using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IngredientPanelManager : MonoBehaviour
{
    Queue<GameObject> ingredientTiles = new();
    [SerializeField] GameObject tilePrefab;


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
