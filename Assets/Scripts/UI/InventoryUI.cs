using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    private bool _activeInventory = false;

    [SerializeField] private GameObject go_SlotsParent;
    private Slot[] slots;
    

    private void Start()
    {
        inventoryPanel.SetActive(_activeInventory);
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }
    
    private void Update()
    {
        // 인벤토리 on / off
        if (Input.GetKeyDown(KeyCode.I))
        {
            _activeInventory = !_activeInventory;
            inventoryPanel.SetActive(_activeInventory);
        }
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        Debug.Log("!!!");
        if(Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)  // null 이라면 slots[i].item.itemName 할 때 런타임 에러 나서
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
    
}
