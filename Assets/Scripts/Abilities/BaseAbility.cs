using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class BaseAbility : MonoBehaviour
{
    [SerializeField] protected Ability parametrs;
    [SerializeField, Range(0, 6)] protected int level = 0;

    public virtual void Apply(Transform target)
    {
        StartCoroutine(Applying(target));
    }
    protected abstract IEnumerator Applying(Transform target);
    public UnityEvent OnComplete;
    public void SetParametrs(Ability parametrs) => this.parametrs = parametrs;

    public virtual void Upgrade()
    {
        if (level + 1 >= 7)
        {
            Debug.LogError("Trying upgrade Ability to over max level");
            return;
        }
        
        level++;
    }
    public float Value => parametrs.LevelValues[level];
    public float ApplyTime => parametrs.applySecondsByLevel[level];
    public float CooldownTime => parametrs.cooldownSecondsByLevel[level];
}