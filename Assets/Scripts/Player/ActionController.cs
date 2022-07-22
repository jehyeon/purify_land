using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField] 
    private InventoryUI inventory;
    
    private Collider2D _clickedEnemy;
    private Collider2D _clickedItem;
    private Collider2D[] _nearEnemies;
    private Collider2D[] _nearItems;
    private LayerMask _enemyLayerMask;
    private LayerMask _itemLayerMask;
    private Vector2 _mousePos;
    private bool _isKeyboardInput; // 키보드와 마우스 동시 움직임 (미구현)
    private GameObject _targetObject;
    public Stat stat;
    public UnitCode unitCode;


    void Start()
    {
        _enemyLayerMask = LayerMask.GetMask("Enemy");
        _itemLayerMask = LayerMask.GetMask("Item");
        stat = new Stat();
        stat = stat.SetUnitStat(unitCode);
    }

    void Update()
    {
        GetMouseCommand();
        GetKeyboardCommand();
    }

    private void FixedUpdate()
    {
        
    }

    GameObject GetClickedObject(Vector2 pos)
    {
        _clickedEnemy = Physics2D.OverlapPoint(pos, _enemyLayerMask); // layerMask 를 조정하여 enemy만 감지하거나 interactive만 감지
        _clickedItem = Physics2D.OverlapPoint(pos, _itemLayerMask); // OverlapPoint(포인트 위치, 감지레이어)
        
        if (_clickedEnemy)
        {
            Debug.Log(_clickedEnemy.ToString());
            return _clickedEnemy.gameObject;
        }
        if (_clickedItem)
        {
            Debug.Log(_clickedItem.ToString());
            return _clickedItem.gameObject;
        }

        return null;
    }

    // 가장 가까운 오브젝트(적, 아이템) 반환
    GameObject GetNearestObject(int keyCode) // keyCode: 원하는 레이어의 오브젝트를 반환하기 위해 사용
    {
        // 주변 적, 아이템을 리스트화 (현재위치, 감지범위, 감지레이어)
        _nearEnemies = Physics2D.OverlapCircleAll(transform.position, 2.0f, _enemyLayerMask);
        _nearItems = Physics2D.OverlapCircleAll(transform.position, 2.0f, _itemLayerMask);

        if (_nearEnemies.Length > 0 && keyCode == 0) // 주변에 적이 있다면
        {
            _nearEnemies.OrderBy(enemy => Vector2.Distance(transform.position, enemy.transform.position)); // 거리순 재정렬
            return _nearEnemies[0].gameObject; // 적의 오브젝트를 반환
        }
        if (_nearItems.Length > 0 && keyCode == 1) // 주변에 아이템이 있다면
        {
            _nearItems.OrderBy(item => Vector2.Distance(transform.position, item.transform.position)); // 거리순 재정렬
            return _nearItems[0].gameObject; // 아이템의 오브젝트를 반환
        }
        return null; // 둘 다 없다면 null 반환
    }
    
    void GetMouseCommand()
    {
        // 마우스 좌클릭: 줍기, 공격
        if (Input.GetMouseButtonDown(0))
        {
            _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 클릭 위치
            _targetObject = GetClickedObject(_mousePos); // 클릭한 오브젝트, 없으면 null 반환
            
            // 이동
            
            // 줍기

            // 공격
        }
        // 마우스 우클릭: 이동
        
        
        // if(_targetObject is null && !_isKeyboardInput)
        //     transform.position = Vector2.MoveTowards(transform.position, _mousePos, Time.deltaTime * 2);     
    }
    
    void GetKeyboardCommand()
    {
        // F: 가까운 몬스터 공격
        if (Input.GetKeyDown(KeyCode.F))
        {
            _targetObject = GetNearestObject(0);
            if (_targetObject is null) return;
            if (_targetObject.CompareTag("Enemy"))
            {
                _targetObject.GetComponent<Enemy>().TakeDamage(stat.attack);
            }
        }
        // 스페이스바: 가까운 아이템 획득 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _targetObject = GetNearestObject(1);
            if (_targetObject is null) return;
            if (_targetObject.CompareTag("Item"))
            {
                inventory.AcquireItem(_targetObject.GetComponent<ItemPickUp>().item);
                Destroy(_targetObject);
            }
        }
    }
}
