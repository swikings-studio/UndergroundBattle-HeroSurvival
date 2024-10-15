using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulsiveField : BaseAbility
{
    [SerializeField] private ParticleSystem effect;
    [SerializeField] private LayerMask neededLayerMask;
    [SerializeField] private Transform target;
    private void Start()
    {
        Apply(target);
    }

    protected override IEnumerator Applying(Transform target)
    {
        var cooldown = new WaitForSeconds(CooldownTime);
        while (true)
        {
            var radius = Value;
            var colliders = Physics.OverlapSphere(target.position, radius, neededLayerMask);

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Enemy") && collider.TryGetComponent(out Rigidbody rigidbody))
                {
                    var direction = collider.transform.position - target.position;
                    rigidbody.AddForce(direction.normalized * ApplyTime, ForceMode.Impulse );
                }
            }

            transform.position = target.position;
            effect.Play();
            yield return cooldown;
        }
        OnComplete?.Invoke();
        yield break;
    }

    private void OnDrawGizmosSelected()
    {
        if (parametrs)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, Value);
        }
    }
}
