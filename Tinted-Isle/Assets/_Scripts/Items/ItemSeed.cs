using TMPro;
using UnityEngine;

public enum eFlower { red, green, yellow, blue, orange, indigo, purple, pink, brown };
public class ItemSeed : Item
{
    [Header("ItemSeed Attributes")]
    [Header("FlowerType")]
    public eFlower flowerType;

    [Header("Seperate prefab for the planted flower")]
    public GameObject flowerPlanted;
}
