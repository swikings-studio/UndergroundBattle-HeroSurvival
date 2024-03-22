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
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }
}