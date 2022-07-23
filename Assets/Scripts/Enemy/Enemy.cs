using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private Rigidbody2D _rigid;
    private Stat _stat;
    public UnitCode unitCode;
    
    public abstract void Skill();
    public abstract void Attack();
    

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _stat = new Stat();
        _stat = _stat.SetUnitStat(unitCode);
    }

    public void TakeDamage(int damage)
    {
        int realDamage = damage - _stat.defense <= 0 
            ? 1 
            : damage - _stat.defense;
        _stat.hp -= realDamage;

        if (_stat.hp <= 0)
        {
            Destroy(this.gameObject);
        }
        
        UpdateHpBar();
    }

    private void UpdateHpBar()
    {
        
    }
}
