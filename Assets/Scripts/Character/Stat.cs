using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    // 공격
    public int Damage { get; private set; }
    public float AttackSpeed { get; private set; }
    public float CriticalPercent { get; private set; }
    // 방어
    public int Defense { get; private set; }
    public int Hp { get; private set; }
    public int MaxHp { get; private set; }
    public int RecoverHp { get; private set; }
    // 기타
    public float Speed { get; private set; }

    // -------------------------------------------------------------------------
    // 초기화
    // -------------------------------------------------------------------------
    public Stat(bool empty = false)
    {
        if (!empty)
        {
            this.Damage = 0;
            this.AttackSpeed = 1f;
            this.CriticalPercent = 0f;
            this.Defense = 0;
            this.Hp = 100;
            this.MaxHp = 100;
            this.RecoverHp = 5;
            this.Speed = 3f;
        }
        else
        {
            this.Damage = 0;
            this.AttackSpeed = 0f;
            this.CriticalPercent = 0f;
            this.Defense = 0;
            this.Hp = 0;
            this.MaxHp = 0;
            this.RecoverHp = 0;
            this.Speed = 0f;
        }
    }

    public void SyncStat(List<Stat> stats)
    {
        // 장착한 아이템으로 현재 스탯 업데이트
        foreach (Stat stat in stats)
        {
            this.Damage += stat.Damage;
            this.AttackSpeed += stat.AttackSpeed;
            this.CriticalPercent += stat.CriticalPercent;
            this.Defense += stat.Defense;
            this.Hp += stat.Hp;
            this.MaxHp += stat.MaxHp;
            this.RecoverHp += stat.RecoverHp;
            this.Speed += stat.Speed;
        }
    }

    // -------------------------------------------------------------------------
    // Stat 수정 (체력만)
    // -------------------------------------------------------------------------
    public void SyncHp(int hp, int maxHp)
    {
        this.Hp = hp;
        this.MaxHp = maxHp;
    }

    public int Attacked(int damage)
    {
        int realDamage = damage - this.Defense > 0
            ? damage - this.Defense
            : 0;

        this.Hp -= realDamage;

        if (this.Hp < 0)
        {
            this.Hp = 0;
        }

        return realDamage;
    }

    public void Recover()
    {
        this.Hp += this.RecoverHp;

        if (this.Hp > this.MaxHp)
        {
            this.Hp = this.MaxHp;
        }
    }

    public void Recover(int amount)
    {
        this.Hp += amount;

        if (this.Hp > this.MaxHp)
        {
            this.Hp = this.MaxHp;
        }
    }

    // -------------------------------------------------------------------------
    // 데미지 계산
    // -------------------------------------------------------------------------

    public int ComputeDamage()
    {
        if (Random.value > this.CriticalPercent)
        {
            return Random.Range((int)(this.Damage * 0.75f), (int)(this.Damage * 1.25f));
        }
        else
        {
            return (int)(this.Damage * 1.25f);
        }
    }
}
