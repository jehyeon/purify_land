using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    private WoodBlock parentWoodBlock;
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void Set(WoodBlock targetObject)
    {
        parentWoodBlock = targetObject;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = (float) parentWoodBlock.stat.hp / parentWoodBlock.stat.maxHp;
    }
}
