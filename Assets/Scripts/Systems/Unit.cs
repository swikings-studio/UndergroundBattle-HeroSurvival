using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class Unit : MonoBehaviour
{
    protected Rigidbody _rigidbody;
    protected Animator _animator;

    public virtual void Awake()
    {
        if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
        if (_animator == null) _animator = GetComponent<Animator>();
    }
    private protected bool CanPlayAnimation()
    {
        AnimatorStateInfo currentAnimatorState = _animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimatorState.IsName("Dash") || currentAnimatorState.IsName("Attack Blend Tree")) return false;
        return true;
    }
}