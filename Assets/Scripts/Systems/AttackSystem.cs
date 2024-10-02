using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public class AttackSystem : BaseSystem, ILockayable
{
    [SerializeField] private bool isAutoAttack;
    [SerializeField, Range(0, 100)] private int damage = 1;
    [Tooltip("Range trying attack"), SerializeField, Range(0, 10)] private float radius = 2;
    [Tooltip("Time in seconds between attack"), SerializeField, Range(0, 10)] private float reload = 0.5f;
    [SerializeField] private AttackType attackType;
    [SerializeField] private LayerMask neededLayersMask;
    [SerializeField] private HitType[] hitVariations;
    [SerializeField] private UnityEvent onStartHit;

    private int currentHitVariation = 0;

    public float Radius => radius;
    public bool IsLocked { get; set; }

    public void StartAttacking()
    {
        attackingCoroutine = StartCoroutine(Attacking());
    }
    public void StopAttacking()
    {
        if (attackingCoroutine != null)
            StopCoroutine(attackingCoroutine);
    }

    private Coroutine attackingCoroutine;
    private bool canAttack => attackingCoroutine != null;

    private void OnEnable()
    {
        onStartHit.AddListener(() => _animator.SetTrigger("Hit"));
        if (hitVariations.Length > 0) onStartHit.AddListener(SwitchHitVariation);
        StartAttacking();
    }
    private void OnDisable()
    {
        onStartHit.RemoveAllListeners();
        StopAttacking();
    }
    private IEnumerator Attacking()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(reload);

        while (true)
        {
            if (isAutoAttack)
            {
                yield return waitForSeconds;

                if (IsLocked) continue;

                onStartHit.Invoke();
            }
            else if (IsTarget())
            {
                yield return waitForSeconds;

                if (IsLocked) continue;

                onStartHit.Invoke();
            }
            yield return null;
        }
    }
    //In radius attack target founded
    private bool IsTarget()
    {
        switch (attackType)
        {
            default:
                return Physics.CheckBox(MeleeAttackCubePosition, Vector3.one * radius, Quaternion.identity, neededLayersMask);
            case AttackType.Around:
            case AttackType.Range:
                return Physics.CheckSphere(transform.position, radius, neededLayersMask);
        }
    }
    private void MeleeAttack()
    {
        Debug.Log(gameObject.name + " Melee Attack");
        Collider[] colliders = Physics.OverlapBox(MeleeAttackCubePosition, Vector3.one * radius / 2f, Quaternion.identity, neededLayersMask);
        AttackDamagableObjects(colliders);
    }
    private void AroundAttack()
    {
        Debug.Log(gameObject.name + " Around Attack");
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, neededLayersMask);
        AttackDamagableObjects(colliders);
    }
    private void RangeAttack()
    {

    }
    private void AttackDamagableObjects(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == gameObject) continue;

            Debug.Log($"{gameObject.name} founded {collider.gameObject.name}");
            if (collider.TryGetComponent(out IDamagable enemy))
            {
                DamageCountText.Create(collider.GetComponent<MonoBehaviour>(), damage);
                Attack(enemy, damage);
            }
        }
    }
    public void AttackWithCurrentAttackType() => GetAttackAction().Invoke();

    private void Attack<T>(T target, int damage) where T : IDamagable
    {
        if (canAttack == false || IsLocked) return;

        Debug.Log($"{gameObject.name} attack {target} with {damage} damage");
        target.GetHit(damage);
    }

    private void SwitchHitVariation()
    {
        if (currentHitVariation >= hitVariations.Length) currentHitVariation = 0;
        HitType currentHitType = hitVariations[currentHitVariation];

        if (currentHitType.HitEffect != null) currentHitType.HitEffect.Play();
        _animator.SetFloat("HitVariation", currentHitType.AnimatorBlendTreeHitThreshold);
        currentHitVariation++;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(MeleeAttackCubePosition, Vector3.one * radius);
    }
    private Vector3 MeleeAttackCubePosition => transform.position + transform.forward * (radius / 2f);

    private UnityAction GetAttackAction() => GetAttackAction(attackType);
    private UnityAction GetAttackAction(AttackType attackType)
    {
        return attackType switch
        {
            AttackType.Around => AroundAttack,
            AttackType.Range => RangeAttack,
            _ => MeleeAttack,
        };
    }
    [System.Serializable]
    private struct HitType
    {
        public ParticleSystem HitEffect;
        public float AnimatorBlendTreeHitThreshold;
    
}

    public override void Upgrade(float value)
    {
        
    }
}