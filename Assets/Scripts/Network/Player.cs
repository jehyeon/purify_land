using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [Header ("For test")]
    [SerializeField]
    private float checkMovedLimit = 0.5f;
    [SerializeField]
    protected float speed = 3;

    public int PlayerId { get; set; }
    public Vector3 DestinationPos;
    public float tempDiff = 0.01f;

    private Vector3 _moveVec;

    protected AnimationController Animator { get; private set; }

    protected void Start()
    {
        this.Animator = GetComponent<AnimationController>();
    }

    protected void Update()
    {
        MoveToPoint();
    }
    
    // -------------------------------------------------------------------------
    // 이동, 회전
    // -------------------------------------------------------------------------
    private void MoveToPoint()
    {
        if ((DestinationPos - this.transform.position).sqrMagnitude < tempDiff)
        // if (this.MoveVec == Vector3.zero)
        {
            this.Animator.IdleAnimation();
            return;
        }

        this.Animator.MoveAnimation(speed);
        this._moveVec = (DestinationPos - this.transform.position).normalized;
        // this.MoveVec = 
        this.transform.position += _moveVec * Time.deltaTime * speed;

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
