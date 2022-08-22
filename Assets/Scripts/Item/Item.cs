using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")] 
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Equipment,
        Consumable,
        Ingredient,
        ETC
    }

    public string itemName;
    public ItemType itemType;
    public Sprite itemImage;
    public GameObject itemPrefab;

    public string weaponType;
}
*/

public enum ItemType
{
    Equipment,
    Consumable,
    Ingredient
}

public class Item
{
    public int Id;
    public string Name;
    public ItemType Type;
    public image Image;
}