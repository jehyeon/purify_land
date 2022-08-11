using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{

    public int Id { get; private set; } = -1;


    // -------------------------------------------------------------------------
    // Set
    // -------------------------------------------------------------------------
    public void Set(int id, Vector3 pos, int hp, int maxHp)
    {
        // !!! hp, maxHp temp
        this.Id = id;
        this.transform.position = pos;
        this.Stat.SyncHp(hp, maxHp);
    }
    public void Reset()
    {
        this.Id = -1;
        this.transform.position = Vector3.zero;
        this.Stat = new Stat();
    }
}
