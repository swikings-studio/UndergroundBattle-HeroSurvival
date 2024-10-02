using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using SwiKinGsStudio.UI;

public class HealthSystem : BaseSystem, IDamagable
{
    [SerializeField] private int healths = 1, resistance = 0;
    [SerializeField] private Slider healthBar;
    public UnityEvent OnGetHit, OnDie;
    private Color normalFillColorHealthBar;
    private LockayablesManager lockayablesManager;

    private int maxHealth;
    private bool _isDied, _isInvulnerability, _isFirstTimeEnable = true;
    public bool IsDied => _isDied;
    private const float actionTime = 0.15f;
    private bool isInjured { get => (float)healths / maxHealth <= 0.4f; }

    private const string getHitAnimationParametrName = "GetHit", isInjuredAnimationParametrName = "isInjured";

    public override void Awake()
    {
        base.Awake();
        if (healthBar != null) normalFillColorHealthBar = healthBar.GetFillColor();
        lockayablesManager = new LockayablesManager(this);
        maxHealth = healths;
    }

    public override void Upgrade(float value)
    {
        maxHealth += (int)value;
        UpdateHealthBar();
    }

    private void OnEnable()
    {
        healthBar?.SmoothlyActivate(actionTime);
        UpdateHealthBar();

        if (_isFirstTimeEnable)
        {
            _isFirstTimeEnable = false;
            return;
        }

        healths = maxHealth;
        _isDied = false;

        lockayablesManager.UnlockAll();
        if (TryGetComponent(out Collider collider)) collider.enabled = true;
    }
    private void Start()
    {
        OnDie.AddListener(Die);
    }
    public void GetHit(int damage)
    {
        if (_isDied || _isInvulnerability) return;

        damage -= resistance;
        if (damage <= 0) damage = 1;

        healths -= damage;

        UpdateHealthBar();
        healthBar?.FillBlink(actionTime / 2f);

        if (healths <= 0)
        {
            OnDie.Invoke();
        }
        else
        {
            _animator.SetBool(isInjuredAnimationParametrName, isInjured);
            if (CanPlayAnimation()) _animator.SetTrigger(getHitAnimationParametrName);
            OnGetHit.Invoke();
        }
    }
    private bool CanPlayAnimation()
    {
        AnimatorStateInfo currentAnimatorState = _animator.GetCurrentAnimatorStateInfo(0);
        return !currentAnimatorState.IsName("Dash") && !currentAnimatorState.IsName("Attack Blend Tree");
    }
    public void Heal(int healCount)
    {
        if (_isDied) return;

        if (healths + healCount > maxHealth) healCount = maxHealth;

        healths += healCount;
        _animator.SetBool(isInjuredAnimationParametrName, isInjured);
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        if (healthBar == null) return;

        float neededValue = (float)healths / maxHealth;

        if (isInjured && healthBar.GetFillColor() != Color.red)
        {
            healthBar.StartFillBlinking(actionTime * 2f);
            healthBar.SetSmoothlyColor(Color.red, actionTime);
        }
        else if (healthBar.GetFillColor() != normalFillColorHealthBar)
        {
            healthBar.StopFillBlinking();
            healthBar.SetSmoothlyColor(normalFillColorHealthBar, actionTime);
        }

        healthBar.SetSmoothlyValue(neededValue, actionTime);
    }
    public void ActivateInvulnerability() => _isInvulnerability = true;
    public void DeactivateInvulnerability() => _isInvulnerability = false;
    private void Die()
    {
        if (TryGetComponent(out Collider collider)) collider.enabled = false;
        if (TryGetComponent(out AttackSystem attackSystem)) attackSystem.StopAttacking();

        _isDied = true;

        healthBar?.StopFillBlinking();
        healthBar?.SmoothlyDisable(actionTime);

        lockayablesManager.LockAll();
        _animator.SetTrigger("Die");
        transform.DOMoveY(-1, 3f).SetDelay(2f).OnComplete(() => gameObject.SetActive(false));
    }
}