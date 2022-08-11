using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private static float _moveAnimationDiff = 0.2f;
    private Animator _animator;
    public Dictionary<int, System.Action> Animations = new Dictionary<int, System.Action>();

    private void Start()
    {
        _animator = GetComponent<Animator>();
        
        // Action 정의
        Animations.Add(1, AttackAnimation);
    }

    public void MoveAnimation(float speed)
    {
        this._animator.SetFloat("moveSpeed", speed * _moveAnimationDiff);
        this._animator.SetBool("isMove", true);
    }

    public void IdleAnimation()
    {
        this._animator.SetBool("isMove", false);
    }


    // -------------------------------------------------------------------------
    // 애니메이션 관리
    // -------------------------------------------------------------------------

    private void AttackAnimation()
    {
        this._animator.SetTrigger("attack");
    }
}
