using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Stat Stat = new Stat();
    private HpBar myHpBar = null;
    protected AnimationController Animator { get; private set; }

    public Vector3 DestinationPos;
    private Vector3 _moveVec;

    protected virtual void Start()
    {
        this.Animator = GetComponent<AnimationController>();
    }

    protected virtual void Update()
    {
        MoveToPoint();
    }

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

    protected virtual void Die()
    {
        Debug.Log("재정의 필요");
    }

    // -------------------------------------------------------------------------
    // 이동, 회전
    // -------------------------------------------------------------------------
    private void MoveToPoint()
    {
        if ((DestinationPos - this.transform.position).sqrMagnitude < 0.01f)
        // if (this.MoveVec == Vector3.zero)
        {
            this.Animator.IdleAnimation();
            return;
        }

        this.Animator.MoveAnimation(this.Stat.Speed);
        this._moveVec = (DestinationPos - this.transform.position).normalized;
        this.transform.position += _moveVec * Time.deltaTime * this.Stat.Speed;

        Rotate(this._moveVec);
    }

    protected bool Rotate(Vector3 direct)
    {
        // 캐릭터 좌우 회전
        if (direct.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);

            return true;
        }
        else if (direct.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        return false;
    }

    public void Rotate(bool right)
    {
        if (right)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // -------------------------------------------------------------------------
    // 애니메이션
    // -------------------------------------------------------------------------
    public void ActAnimation(int actionType)
    {
        System.Action animationFunc = null;

        if (Animator.Animations.TryGetValue(actionType, out animationFunc))
        {
            animationFunc();
        }
    }
}
