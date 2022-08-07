using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Stat Stat = new Stat();
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
        this.Stat.SyncHp(hp, maxHp);

        UpdateHpBar((float)this.Stat.Hp / (float)this.Stat.MaxHp);
    }

    public int[] TakeDamage(int damage)
    {
        // 받은 데미지만큼 hp, maxHp 업데이트 후 return
        this.Stat.Attacked(damage);

        return new int[] { this.Stat.Hp, this.Stat.MaxHp };
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
