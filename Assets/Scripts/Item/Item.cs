using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Equipment = 0,
    Consumable = 1,
    Ingredient = 2
}

public class Item
{
    public int Id;
    public string Name;
    public ItemType Type;
    public Image Image;

    public Item(int itemId, string itemName, int itemType)
    {
        this.Id = itemId;
        this.Name = itemName;
        this.Image.sprite = Resources.Load<Sprite>($"Datas/Images/{itemId}");
        this.Type = (ItemType)itemType;
    }

    //TEMP
    public Item(Dictionary<string, object> data)
    {
        this.Id = (int)data["id"];
        this.Name = (string)data["name"];
        this.Image.sprite = Resources.Load<Sprite>($"Datas/Images/{this.Id}");
        this.Type = (ItemType)(int)data["type"];
    }
}