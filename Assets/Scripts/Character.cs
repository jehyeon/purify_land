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
    //protected void Attack(Character target)
    //{
    //    // !!! 임시
    //    int tempDamage = Random.Range(5, 11);
    //    target.Attacked(tempDamage);
    //}
    
    public void SyncHp(int hp, int maxHp)
    {
        // !!! _stat.maxHp와 maxHp가 잘못될 경우
        _stat.hp = hp;
        _stat.maxHp = maxHp;

        if (_stat.hp < 0)
        {
            _stat.hp = 0;
        }

        UpdateHpBar((float)_stat.hp / (float)_stat.maxHp);
    }

    public int[] TakeDamage(int damage)
    {
        // 받은 데미지만큼 hp, maxHp 업데이트 후 return
        _stat.hp -= damage;

        if (_stat.hp < 0)
        {
            _stat.hp = 0;
        }

        return new int[] { _stat.hp, _stat.maxHp };
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
