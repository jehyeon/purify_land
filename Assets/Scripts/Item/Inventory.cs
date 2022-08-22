using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Transform _slotParent;

    public Item item;
    public Dictionary<int, InventorySlot> Slots { get; private set; }
    
    private int _nextSlotId = 0;


    private void Start()
    {

    }

    public void Load(List<Dictionary<string, int>> items)
    {
        this.Clear();
        // from server
        // [{'id': itemId, 'count': count}, {}, {}]
        foreach (Dictionary<string, int> item in items)
        {
            // !!! 오래 걸릴 수 있음
            //Add(item["id"], item["count"], false);   // !!! error
        }
    }

    private void Clear()
    {
        _nextSlotId = 0;
        this.Slots = new Dictionary<int, InventorySlot>();

        InventorySlot[] slots = _slotParent.GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot slot in slots)
        {
            slot.Clear();
            this.Slots.Add(_nextSlotId, slot);
            _nextSlotId += 1;
        }
    }

    public void Save()
    {
        // to server
    }

    public void Add(Item item, int itemCount, bool save = true)
    {
        if (item.Type != ItemType.Equipment)
        {
            // 장착 아이템이 아닌 경우, 동일한 아이템이 있을 때 수량만 업데이트
            int slotId;
            if (this.Find(item.Id, out slotId))
            {
                this.Slots[slotId].UpdateCount(itemCount);

                if (save)
                {
                    // save
                }

                return;
            }
        }

        //GameObject goSlot = GameManager.Instance.CreateGO("Prefabs/UI/InventorySlot", _slotParent);
        //InventorySlot slot = goSlot.GetComponent<InventorySlot>();
        //slot.Set(item, itemCount);

        //this.Slots.Add(nextSlotId, slot);

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

    // -------------------------------------------------------------------------
    // 검색
    // -------------------------------------------------------------------------
    public bool Find(int itemId, out int slotId)
    {
        //찾으면 slotId 할당 및 return true
        slotId = -1;
        return false;
    }

}
