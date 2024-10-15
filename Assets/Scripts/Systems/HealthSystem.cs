using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using SwiKinGsStudio.UI;

public class HealthSystem : BaseHealthSystem
{
    [SerializeField] private Slider healthBar;
    private Color _normalFillColorHealthBar;
    private LockayablesManager lockayablesManager;

    private bool _isFirstTimeEnable = true;
    private const float actionTime = 0.15f;

    private const string getHitAnimationParametrName = "GetHit", isInjuredAnimationParametrName = "isInjured";

    public override void Awake()
    {
        base.Awake();
        if (healthBar != null) _normalFillColorHealthBar = healthBar.GetFillColor();
        lockayablesManager = new LockayablesManager(this);
    }

    public override void Upgrade(float value)
    {
        base.Upgrade(value);
        UpdateHealthBar();
    }

    protected override void OnEnable()
    {
        healthBar?.SmoothlyActivate(actionTime);
        UpdateHealthBar();

        if (_isFirstTimeEnable)
        {
            _isFirstTimeEnable = false;
            return;
        }
        base.OnEnable();

        lockayablesManager.UnlockAll();
        _collider.enabled = true;
    }
    public override void GetHit(int damage)
    {
        base.GetHit(damage);
        
        UpdateHealthBar();
        healthBar?.FillBlink(actionTime / 2f);

        if (healths <= 0)
        {
            OnHealthsOver.Invoke();
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
    public override void Heal(int healCount)
    {
        base.Heal(healCount);
        
        _animator.SetBool(isInjuredAnimationParametrName, isInjured);
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        if (healthBar is null) return;

        float neededValue = (float)healths / maxHealth;

        if (isInjured && healthBar.GetFillColor() != Color.red)
        {
            healthBar.StartFillBlinking(actionTime * 2f);
            healthBar.SetSmoothlyColor(Color.red, actionTime);
        }
        else if (healthBar.GetFillColor() != _normalFillColorHealthBar)
        {
            healthBar.StopFillBlinking();
            healthBar.SetSmoothlyColor(_normalFillColorHealthBar, actionTime);
        }

        healthBar.SetSmoothlyValue(neededValue, actionTime);
    }

    protected override void Die()
    {
        if (TryGetComponent(out AttackSystem attackSystem)) attackSystem.StopAttacking();
        
        base.Die();

        healthBar?.StopFillBlinking();
        healthBar?.SmoothlyDisable(actionTime);

        lockayablesManager.LockAll();
        _animator.SetTrigger("Die");
        transform.DOMoveY(-1, 3f).SetDelay(_animator.GetCurrentAnimatorStateInfo(0).length).OnComplete(() => gameObject.SetActive(false));
    }

}