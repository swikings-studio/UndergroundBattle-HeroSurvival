using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AttackSystem : BaseSystem, ILockayable
{
    [SerializeField] private bool isAutoAttack;
    [SerializeField] private List<AttackUpgrade> upgrades;
    [SerializeField] private LayerMask neededLayersMask;
    [SerializeField] private Transform attackUpgradesContainer, activeWeaponsContainer;
    
    public float RadiusCurrentAttackUpgrade => CurrentAttackUpgrade ? CurrentAttackUpgrade.Radius : 1;
    
    private AttackUpgrade CurrentAttackUpgrade => upgrades[_currentUpgradeIndex];
    private readonly Dictionary<string, GameObject> _instantiatedUpgradesByTitle = new ();
    private int _currentUpgradeIndex;

    public bool IsLocked { get; set; }

    private void StartAttacking()
    {
        _attackingCoroutine = StartCoroutine(Attacking());
    }

    public void StopAttacking()
    {
        if (_attackingCoroutine == null) return;

        StopCoroutine(_attackingCoroutine);
        _attackingCoroutine = null;
    }

    private Coroutine _attackingCoroutine;
    private bool CanAttack => _attackingCoroutine != null;

    private void OnEnable()
    {
        StartAttacking();
    }

    private void OnDisable()
    {
        StopAttacking();
    }

    private IEnumerator Attacking()
    {
        if (upgrades.Count == 0)
        {
            StopAttacking();
            yield break;
        }

        var waitForUnlock = new WaitUntil(() => IsLocked == false);

        while (upgrades.Count > 0)
        {
            var waitForReload = new WaitForSeconds(CurrentAttackUpgrade.Reload);

            if (isAutoAttack)
            {
                yield return waitForReload;

                yield return waitForUnlock;

                yield return StartCoroutine(UseCurrentAttackUpgrade());
            }
            else if (IsTarget())
            {
                yield return waitForReload;

                yield return waitForUnlock;

                yield return StartCoroutine(UseCurrentAttackUpgrade());
            }

            NextAttackUpgrade();
            yield return null;
        }
    }

    private void NextAttackUpgrade()
    {
        if (_currentUpgradeIndex + 1 >= upgrades.Count) _currentUpgradeIndex = 0;
        else _currentUpgradeIndex++;
    }

    public void AddAttackUpgrade(AttackUpgrade newUpgrade)
    {
        if (upgrades.Contains(newUpgrade)) return;
        
        upgrades.Add(newUpgrade);
    }

    private IEnumerator UseCurrentAttackUpgrade()
    {
        switch (CurrentAttackUpgrade)
        {
            case Weapon weapon:
                if (weapon.animationVariation == HitVariation.None)
                {
                    AttackAction();
                }
                else
                {
                    _animator.SetFloat(AnimatorHitVariationFloatName, weapon.AnimationHitVariation);
                    _animator.SetTrigger(AnimatorHitTriggerName);
                }
                
                if (weapon.reference.RuntimeKeyIsValid())//TODO: Load from AddressableManager
                {
                    if (_instantiatedUpgradesByTitle.TryGetValue(weapon.Title, out var weaponObject) == false)
                    {
                        var container = weapon.spawnAsWeapon ? activeWeaponsContainer : attackUpgradesContainer;
                        var operationHandle = Addressables.InstantiateAsync(weapon.reference, container);
                        if (!operationHandle.IsDone) yield return operationHandle;
                        
                        if (operationHandle.Status == AsyncOperationStatus.Succeeded)
                        {
                            weaponObject = operationHandle.Result;
                        }
                        else
                        {
                            Debug.LogError("Error Instantiate weapon");
                            operationHandle.Release();
                            yield break;
                        }
                        
                        StartCoroutine(weapon.EnableParticlesWithDelay(weaponObject));
                        _instantiatedUpgradesByTitle.Add(weapon.Title, weaponObject);
                    }
                    else
                    {
                        weaponObject.SetActive(true);
                        
                        StartCoroutine(weapon.EnableParticlesWithDelay(weaponObject));
                        
                        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                        
                        weaponObject.SetActive(false);
                    }
                    
                }
                break;
            case Ability ability:
                break;
        }
    }

    /// <summary>
    /// Find target in radius
    /// </summary>
    /// <returns>In radius attack target founded</returns>
    private bool IsTarget()
    {
        if (CurrentAttackUpgrade is Weapon weapon)
            switch (weapon.attackType)
            {
                default:
                    return Physics.CheckBox(MeleeAttackCubePosition, Vector3.one * CurrentAttackUpgrade.Radius,
                        Quaternion.identity, neededLayersMask);
                case AttackType.Around:
                case AttackType.Range:
                    return Physics.CheckSphere(transform.position, CurrentAttackUpgrade.Radius, neededLayersMask);
            }

        return CurrentAttackUpgrade is Ability; //TODO: Check after ability radius set
    }

    private void MeleeAttack()
    {
        Debug.Log(gameObject.name + " Melee Attack");
        Collider[] colliders = Physics.OverlapBox(MeleeAttackCubePosition,
            Vector3.one * CurrentAttackUpgrade.Radius / 2f, Quaternion.identity, neededLayersMask);
        AttackDamagableObjects(colliders);
    }

    private void AroundAttack()
    {
        Debug.Log(gameObject.name + " Around Attack");
        Collider[] colliders =
            Physics.OverlapSphere(transform.position, CurrentAttackUpgrade.Radius, neededLayersMask);
        AttackDamagableObjects(colliders);
    }

    private void RangeAttack()
    {
        Debug.Log(gameObject.name + " Range Attack");
    }

    private void AttackDamagableObjects(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == gameObject) continue;

            Debug.Log($"{gameObject.name} founded {collider.gameObject.name}");
            if (collider.TryGetComponent(out IDamagable enemy))
            {
                Attack(enemy, CurrentAttackUpgrade.ValueInt);
            }
        }
    }

    private void Attack<T>(T target, int damage) where T : IDamagable
    {
        if (CanAttack == false || IsLocked) return;

        Debug.Log($"{gameObject.name} attack {target} with {damage} damage");
        target.GetHit(damage);
    }

    private void OnDrawGizmosSelected()
    {
        if (CurrentAttackUpgrade && CurrentAttackUpgrade is Weapon currentAttackWeapon)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(MeleeAttackCubePosition, Vector3.one * currentAttackWeapon.Radius);
        }
    }

    private Vector3 MeleeAttackCubePosition =>
        transform.position + transform.forward * (CurrentAttackUpgrade.Radius / 2f);

    /// <summary>
    /// Applying from hit animation clip if current attack upgrade have it
    /// </summary>
    private void AttackAction()
    {
        if (CurrentAttackUpgrade is Weapon weapon)
            switch (weapon.attackType)
            {
                case AttackType.Melee:
                    MeleeAttack();
                    break;
                case AttackType.Around:
                    AroundAttack();
                    break;
                case AttackType.Range:
                    RangeAttack();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        else if (CurrentAttackUpgrade is Ability ability)
        {
        }
    }
}