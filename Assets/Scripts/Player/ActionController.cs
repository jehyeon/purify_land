using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    private Collider2D[] _nearEnemies;
    private Collider2D[] _nearInteractives;
    private LayerMask _enemyLayer;
    private LayerMask _InteractiveLayer;

    void Start()
    {
        _enemyLayer = LayerMask.GetMask("Enemy");
        _InteractiveLayer = LayerMask.GetMask("Item");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Action(GetNearObject());
            
        }
    }

    Collider2D GetNearObject()
    {
        // layerMask 를 조정하여 enemy만 감지하거나 interactives만 감지
        _nearEnemies = Physics2D.OverlapCircleAll(transform.position, 5.0f, _enemyLayer);
        _nearInteractives = Physics2D.OverlapCircleAll(transform.position, 2.0f, _InteractiveLayer);
        
        // 근처 아이템(상호작용 가능한 오브젝트)이 있다면 (아이템이 우선순위가 더 높다고 판단, 대신 아이템은 더 좁은 감지범위)
        if (_nearInteractives.Length > 0)
        {
            // 거리순 재정렬
            _nearInteractives.OrderBy(interactive => Vector2.Distance(transform.position, interactive.transform.position));
            foreach (var cols in _nearInteractives)
            {
                Debug.Log(cols);
                return cols;
            }
        }
        
        // 근처 적이 있다면
        if(_nearEnemies.Length > 0)
        {
            // 거리순 재정렬
            _nearEnemies.OrderBy(enemy => Vector2.Distance(transform.position, enemy.transform.position));
            foreach(var cols in _nearEnemies)
            {
                Debug.Log(cols);
                return cols;
            }
        }

        return null;
    }
    
    // Action 에서 Attack, GetItem, Interaction을 호출하여 적절한 행동을 취함
    // target의 layerMask 에 따라 다르게 행동하는 방식으로 구현하면 될 듯
    void Action(Collider2D target)
    {

    }

    // 공격
    void Attack()
    {
        
    }
    
    // 아이템 줍기
    void GetItem()
    {
        
    }
    
    // 각종 사물이나 NPC 상호작용
    void Interaction()
    {
        
    }
    
}
