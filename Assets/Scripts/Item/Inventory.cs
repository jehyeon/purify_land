using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item item;
    public Dictionary<int, InventorySlot> Slots { get; private set; }
    
    private int _nextSlotId = 0;

    [SerializeField]
    private Transform _slotParent;

    private void Start()
    {

    }

    public void Load(List<Dictionary<int, int>> items)
    {
        this.Clear();
        // from server
        // [{'id': itemId, 'count': count}, {}, {}]
        foreach (Dictionary<int, int> item in items)
        {
            // !!! 오래 걸릴 수 있음
            Add(item.key, item.count, false);   // !!! error
        }
    }

    private void Clear()
    {
        _nextSlotId = 0;
        this.Slots = new Dictionary<int, InventorySlot>();
    }

    public void Save()
    {
        // to server
    }

    public void Add(int itemId, int itemCount, bool save = true)
    {
        GameObject goSlot = GameManager.Instance.CreateGO("Prefabs/UI/InventorySlot", _slotParent);
        InventorySlot slot = goSlot.GetComponent<InventorySlot>().Set(itemId, itemCount);
        this.Slots.Add(nextSlotId, slot);

        _nextSlotId += 1;

        if (save)
        {
            // save
        }
    }

    public void Remove(int slotId)
    {
        // remove dictionary
        this.Slots[slotId].gameObject.SetActive(false);
        
        if (this.Slots.Remove(slotId))
        {
            // save
        }
        else
        {
            // error
        }
    }
}
