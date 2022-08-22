using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private Text itemCount;

    public Item Item { get; private set; }
    public int Count { get; private set; }
    
    private void SetColor(float alpha)
    {
        // 아이템 이미지 투명도 조절
        Color color = Item.Image.color;
        color.a = alpha;
        this.Item.Image.color = color;
    }

    public void Set(Item item, int count = 1)
    {
        // 인벤토리 슬롯에 아이템 추가
        this.Item = item;
        this.Count = count;
        this.itemImage = this.Item.Image;
        SetColor(1);
        UpdateItemCountText();
    }
    
    public void UpdateCount(int count)
    {
        // 아이템 갯수 업데이트
        this.Count += count;
        UpdateItemCountText();

        // 미구현
    }

    private void UpdateItemCountText()
    {
        if (this.Count == 0)
        {
            itemCount.text = "";
        }
        else
        {
            itemCount.text = $"{this.Count}";
        }
    }

    public void Clear()
    {
        // 슬롯의 아이템 삭제
        this.Item = null;
        this.Count = 0;
        SetColor(0);
        UpdateItemCountText();
    }
}
