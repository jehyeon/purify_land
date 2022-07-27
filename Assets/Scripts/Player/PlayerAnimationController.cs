using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private static float _moveAnimationDiff = 0.2f;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
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
}
