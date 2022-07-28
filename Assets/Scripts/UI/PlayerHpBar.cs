using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    public Slider bar;
    private Stat pStat;
    private Player player;
    void Start()
    {
        // pStat = player.stat;
    }
    
    void Update()
    {
        bar.value = (float)pStat.hp / pStat.maxHp;
    }
}
