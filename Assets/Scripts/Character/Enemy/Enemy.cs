using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle = 0,
    Detect = 1,
    Attack = 2,
    Back = 3,
}

public abstract class Enemy : Character
{
    public int Id { get; private set; } = -1;
    public Vector3 StartPos { get; private set; }
    //public Player Taget { get; set; }
    public Player Target;       // !!!

    protected float _backRange;
    protected float _attackRange;
    public State State { get; set; }

    public float BackRange { get { return _backRange; } }

    protected override void Update()
    {
        if (this.State == State.Detect && this.Target != null)
        {
            // Detect 중 일때에는 target을 계속 따라감
            this.DestinationPos = this.Target.transform.position;
        }

        base.Update();
    }

    // -------------------------------------------------------------------------
    // Target
    // -------------------------------------------------------------------------
    public void SetTarget(Player target)
    {
        this.Target = target;
    }

    // -------------------------------------------------------------------------
    // State
    // -------------------------------------------------------------------------
    public void SetState(int state)
    {
        switch ((State)state)
        {
            case State.Back:
                BackMode();
                break;
            case State.Idle:
                IdleMode();
                break;
        }
    }

    private void BackMode()
    {
        this.DestinationPos = this.StartPos;
        this.Target = null;
        this.State = State.Back;
    }

    private void IdleMode()
    {
        this.State = State.Idle;
    }

    //public void DetectMode()
    //{
    //    this.State = State.Detect;
    //}

    //private void AttackMode()
    //{
    //    _enemy.State = State.Attack;
    //}

    // -------------------------------------------------------------------------
    // Set
    // -------------------------------------------------------------------------
    public void Set(int id, Vector3 pos, int hp, int maxHp)
    {
        // !!! hp, maxHp temp
        this.Id = id;
        this.transform.position = pos;
        this.DestinationPos = pos;
        this.StartPos = pos;
        this.Stat.SyncHp(hp, maxHp);
    }

    public void Reset()
    {
        this.Id = -1;
        this.transform.position = Vector3.zero;
        this.Stat = new Stat();
        this.State = State.Idle;
    }
}
