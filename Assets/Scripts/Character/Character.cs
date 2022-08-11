using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Stat Stat = new Stat();
    private HpBar myHpBar = null;

    // -------------------------------------------------------------------------
    // 체력
    // -------------------------------------------------------------------------
    public void SyncHp(int hp, int maxHp)
    {
        this.Stat.SyncHp(hp, maxHp);

        UpdateHpBar((float)this.Stat.Hp / (float)this.Stat.MaxHp);
    }

    public int[] TakeDamage(int damage)
    {
        // 받은 데미지만큼 hp, maxHp 업데이트 후 return
        this.Stat.Attacked(damage);

        if (this.Stat.Hp == 0)
        {
            Die();
        }

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

    // -------------------------------------------------------------------------
    // 사망
    // -------------------------------------------------------------------------
    protected virtual void Die()
    {
        Debug.Log("재정의 필요");
    }
}
