using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealField : BaseAbility
{
    [SerializeField] private ParticleSystem effect;
    protected override IEnumerator Applying(Transform target)
    {
        effect.Play();
        OnComplete?.Invoke();
        yield break;
    }
}
