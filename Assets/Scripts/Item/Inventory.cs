using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Transform _slotParent;
    public Dictionary<int, InventorySlot> Slots { get; private set; } // < slotId, slot >
    public Dictionary<int, int> OwnedItemsDict; // < itemId, slotId >

    private int _nextSlotId = 0;
    private int _firstEmptySlotId = 0;
    private bool _isFull = false;


    private void Start()
    {
        OwnedItemsDict = new Dictionary<int, int>();
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
        if (item.Type == ItemType.Equipment)
        {
            if (_isFull)
            {
                return;
            }
            
            // 장착 아이템인 경우, 빈 칸에 추가
            this.OwnedItemsDict.Add(item.Id, _firstEmptySlotId);
            this.Slots[_firstEmptySlotId].Set(item);
            _firstEmptySlotId = this.GetEmptySlot();
        }
        else
        {
            // 장착 아이템이 아닌 경우, 동일한 아이템이 있을 때 수량만 업데이트
            if (this.Find(item.Id, out var slotId))
            {
                this.Slots[slotId].UpdateCount(itemCount);

                if (save)
                {
                    // save
                }

                return;
            }
            else
            {
                if (_isFull)
                {
                    return;
                }
                
                slotId = _firstEmptySlotId;
                this.OwnedItemsDict.Add(item.Id, slotId);
                this.Slots[slotId].Set(item, itemCount);
                _firstEmptySlotId = this.GetEmptySlot();
            }
        }

        //GameObject goSlot = GameManager.Instance.CreateGO("Prefabs/UI/InventorySlot", _slotParent);
        //InventorySlot slot = goSlot.GetComponent<InventorySlot>();
        //slot.Set(item, itemCount);
        //this.Slots.Add(nextSlotId, slot);
        // _nextSlotId += 1;

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
        if (this.OwnedItemsDict.ContainsKey(itemId))
        {
            slotId = this.OwnedItemsDict[itemId];
            return true;
        }
        else
        {
            slotId = -1;
            return false;
        }
    }

    private int GetEmptySlot()
    {
        foreach (var slot in Slots)
        {
            if (slot.Value.Count == 0)
            {
                _isFull = false;
                return slot.Key;
            }
        }

        _isFull = true;
        return -1;
    }
    
    // -------------------------------------------------------------------------
    // 추가 해야할 것: 나누기, 합치기, 정렬(+합치기)
    // -------------------------------------------------------------------------
}
