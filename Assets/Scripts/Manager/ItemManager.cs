using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static List<Dictionary<string, object>> _data;
    private static ItemManager _instance = null;
    public static ItemManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ItemManager();
            }

            return _instance;
        }
    }

    public Item Get(int itemId)
    {
        if (_data == null)
        {
            _data = CSVReader.Read("Datas/Item");
        }

        return new Item(_data[itemId]);
    }
}