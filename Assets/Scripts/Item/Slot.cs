using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item Item { get; private set; }
    public int Count { get; private set; }
    
    // 아이템 이미지 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 인벤토리 슬롯에 아이템 추가
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;
        SetColor(1);
    }
    
    // 아이템 갯수 업데이트
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        // 미구현
    }

    // 슬롯의 아이템 삭제
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
    }


}
