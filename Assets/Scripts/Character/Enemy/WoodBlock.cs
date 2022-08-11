using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WoodBlock : MonoBehaviour
{
    private Rigidbody2D rigid;
    public Stat stat;
    //public UnitCode unitCode;

    // temp
    [SerializeField]
    private GameObject hpBarParent;
    [SerializeField]
    private GameObject hpBarPref;
    private GameObject hpBarObject;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        stat = new Stat();
        //stat = stat.SetUnitStat(unitCode);
    }

    private void Update()
    {
        if (hpBarObject)
        {
            hpBarObject.transform.position =
                Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 1.0f, 0));
        }
    }

    public void TakeDamage(int damage)
    {
        //int realDamage = damage - stat.defense <= 0 
        //    ? 1 
        //    : damage - stat.defense;
        //stat.hp -= realDamage;

        //if (stat.hp <= 0)
        //{
        //    Destroy(hpBarObject);
        //    Destroy(this.gameObject);
        //}

        UpdateHpBar();
    }

    private void UpdateHpBar()
    {
        if (hpBarObject is null)
        {
            hpBarObject = Instantiate(hpBarPref, hpBarParent.transform);
            // hpBarObject.transform.SetParent(hpBarParent.transform);
            hpBarObject.GetComponent<EnemyHpBar>().Set(this);
        }

    }
}
