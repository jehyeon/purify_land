using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    private WoodBlock parentWoodBlock;
    private Slider _hpBar;

    private void Start()
    {
        _hpBar = GetComponent<Slider>();
        _hpBar.transform.position = parentWoodBlock.transform.position + Vector3.up;
    }

    public void Set(WoodBlock targetObject)
    {
        parentWoodBlock = targetObject;
    }

    // public void RefreshHpBar()
    // {
    //     _hpBar.value = (float) parentWoodBlock.stat.hp / parentWoodBlock.stat.maxHp;
    // }

    // Update is called once per frame
    void Update()
    {
        _hpBar.value = (float) parentWoodBlock.stat.hp / parentWoodBlock.stat.maxHp;
    }
}
