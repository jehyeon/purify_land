using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header ("For test")]
    [SerializeField]
    private float checkMovedLimit = 0.5f;
    [SerializeField]
    protected float speed = 3;

    public int PlayerId { get; set; }
    public Vector3 MoveVec { get; set; }
    public Vector3 DetinationPos;
    public float tempDiff = 0.01f;

    // private Stat _stat;
    // private UnitCode _unitCode;

    protected PlayerAnimationController Animator { get; private set; }

    protected void Start()
    {
        this.Animator = GetComponent<PlayerAnimationController>();
        // _stat = new Stat();
        // _stat = _stat.SetUnitStat(_unitCode);
    }

    protected void Update()
    {
        Move();
    }
    
    // -------------------------------------------------------------------------
    // 이동
    // -------------------------------------------------------------------------
    protected void Move()
    {
        if ((DetinationPos - this.transform.position).sqrMagnitude < tempDiff)
        {
            // 이동 벡터가 없는 경우
            Stop();
            return;
        }

        this.Animator.MoveAnimation(speed);
        this.MoveVec = (DetinationPos - this.transform.position).normalized;
        this.transform.position += this.MoveVec * Time.deltaTime * speed;

        // 캐릭터 좌우 회전
        if (MoveVec.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (MoveVec.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Stop()
    {
        MoveVec = Vector3.zero;
        this.Animator.IdleAnimation();
    }

    // private void MoveToPoint()
    // {
    //     Vector2 myPosition = this.transform.position;
    //     if ((myPosition - DestinationPos).sqrMagnitude < checkMovedLimit)
    //     {
    //         Rigid.velocity = Vector2.zero;
    //         this.Animator.IdleAnimation();
    //     }
    //     else
    //     {
    //         // DestinationPos로 이동 !!!
    //         Vector2 moveVec = (DestinationPos - myPosition).normalized;
    //         Rigid.velocity = moveVec * 10;   // !!!

    //         this.Animator.MoveAnimation();
    //     }
    // }
}
