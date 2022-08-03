using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Stat _stat = new Stat();
    private HpBar myHpBar = null;

    // -------------------------------------------------------------------------
    // 공격, 피격
    // -------------------------------------------------------------------------
    protected void Attack(Character target)
    {
        // !!! 임시
        int tempDamage = Random.Range(5, 11);
        target.Attacked(tempDamage);
    }
    
    public void Attacked(int damage)
    {
        _stat.hp -= damage;

        if (_stat.hp < 0)
        {
            _stat.hp = 0;
        }

        UpdateHpBar((float)_stat.hp / (float)_stat.maxHp);
    }

    private void UpdateHpBar(float percent)
    {
        if (myHpBar == null)
        {
            myHpBar = UIManager.Instance.CreateHpBar();
            Vector3 offset = new Vector3(0, 65f, 0);
            myHpBar.SetTarget(this.transform, offset);
        }

        myHpBar.UpdateHpBar(percent);
    }
}
