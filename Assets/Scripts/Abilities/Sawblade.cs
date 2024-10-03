using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawblade : BaseAbility
{
    [SerializeField] private Transform target;
    private void Start()
    {
        Apply(target);
    }

    public override void Apply(Transform target)
    {
        StartCoroutine(Applying(target));
    }

    protected override IEnumerator Applying(Transform target)
    {
        //float applyTyme = Parametrs.applySecondsByLevel[level];
        float applyTyme = 5;
        float degreesPerSecond = 360 / applyTyme;
        
        while (applyTyme > 0)
        {
            applyTyme -= Time.deltaTime;
            transform.RotateAround(target.position, Vector3.up, degreesPerSecond * Time.deltaTime);
            yield return null;
        }
    }
}
