using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WoodBlock : MonoBehaviour
{
    private Rigidbody2D rigid;
    public Stat stat;
    public UnitCode unitCode;

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
        stat = stat.SetUnitStat(unitCode);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        int realDamage = damage - stat.defense <= 0 
            ? 1 
            : damage - stat.defense;
        stat.hp -= realDamage;

        if (stat.hp <= 0)
        {
            Destroy(this.gameObject);
        }

        UpdateHpBar();
    }

    private void UpdateHpBar()
    {
        if (hpBarObject == null)
        {
            hpBarObject = Instantiate(hpBarPref);
            hpBarObject.transform.parent = hpBarParent.transform;
            hpBarObject.GetComponent<EnemyHpBar>().Set(this);
        }
    }
}
