using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public abstract class BaseSystem : MonoBehaviour
{
    protected Rigidbody _rigidbody;
    protected Animator _animator;

    public virtual void Awake()
    {
        if (_rigidbody == null)
        {
            Debug.Log("Get Rigidbody");
            _rigidbody = GetComponent<Rigidbody>();
        }

        if (_animator == null)
        {
            Debug.Log("Get Animator");
            _animator = GetComponent<Animator>();
        }
    }

    public abstract void Upgrade(float value);
}