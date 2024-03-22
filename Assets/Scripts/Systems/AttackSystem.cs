using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackSystem : Unit, ILockayable
{
    [SerializeField] private bool isAutoAttack;
    [SerializeField, Range(0, 100)] private int damage = 1;
    [Tooltip("Радиус попытки атаки"), SerializeField, Range(0, 10)] private float radius = 2;
    [Tooltip("Время в секундах между попытками атаки"), SerializeField, Range(0, 10)] private float reload = 0.5f;
    [SerializeField] private AttackType attackType;
    [SerializeField] private LayerMask neededLayersMask;
    [SerializeField] private HitType[] hitTypes;
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

    private void Start()
    {
        onStartHit.AddListener(() => _animator.SetTrigger("Hit"));
        onStartHit.AddListener(SwitchHitType);
    }

    private void OnEnable()
    {
        StartAttacking();
    }
    private IEnumerator Attacking()
    {
        while (true)
        {
            if (isAutoAttack)
            {
                yield return new WaitForSeconds(reload);

                if (IsLocked) continue;

                onStartHit.Invoke();
            }
            else if (IsTarget())
            {
                yield return new WaitForSeconds(reload);

                if (IsLocked) continue;

                onStartHit.Invoke();
            }
            yield return null;
        }
    }
    //В радиусе атаки кто-то есть
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
        AttackDamableObjects(colliders);
    }
    private void AroundAttack()
    {
        Debug.Log(gameObject.name + " Around Attack");
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, neededLayersMask);
        AttackDamableObjects(colliders);
    }
    private void RangeAttack()
    {

    }
    private void AttackDamableObjects(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == gameObject) continue;

            Debug.Log($"{gameObject.name} founded {collider.gameObject.name}");
            if (collider.TryGetComponent(out IDamagable enemy))
            {
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

    private void SwitchHitType()
    {
        if (currentHitVariation >= hitTypes.Length) currentHitVariation = 0;
        HitType currentHitType = hitTypes[currentHitVariation];

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
    public enum AttackType
    {
        Melee,
        Around,
        Range
    }
}