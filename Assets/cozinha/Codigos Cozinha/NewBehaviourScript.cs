using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="FoodItem", menuName = "PI Abacookie/Food Item")]
public class FoodItem : ScriptableObject
{
    public string itemName;
    public int calories;
    public Sprite image;
    public bool isHealthy;
}
