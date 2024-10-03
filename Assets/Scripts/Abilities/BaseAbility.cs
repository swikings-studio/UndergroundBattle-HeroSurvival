using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    [SerializeField] protected Ability Parametrs;
    protected int level = 1;
    public abstract void Apply(Transform target);
    protected abstract IEnumerator Applying(Transform target);

    public void SetParametrs(Ability parametrs) => Parametrs = parametrs;
    public float Value => Parametrs.LevelValues[level];
}