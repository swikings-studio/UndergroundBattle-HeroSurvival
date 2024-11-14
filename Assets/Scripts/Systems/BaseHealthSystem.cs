using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(Collider))]
public class BaseHealthSystem : BaseSystem, IDamagable, IUpgradable
{
    [SerializeField] protected int healths = 1, resistance = 0;
    public UnityEvent OnGetHit, OnHealthsOver;
    
    protected int maxHealth;
    protected bool _isDied, _isInvulnerability;
    public bool IsDied => _isDied;
    public bool IsInvulnerability => _isInvulnerability;
    protected bool isInjured { get => (float)healths / maxHealth <= 0.4f; }

    protected Collider _collider;
    public override void Awake()
    {
        base.Awake();
        maxHealth = healths;
        _collider = GetComponent<Collider>();
    }

    public PlayerSystem PlayerSystem { get; } = PlayerSystem.Health;

    public virtual void Upgrade(float value)
    {
        maxHealth += (int)value;
    }
    protected virtual void Start()
    {
        OnHealthsOver.AddListener(Die);
    }

    protected virtual void OnEnable()
    {
        healths = maxHealth;
        _isDied = false;
    }
    public void ActivateInvulnerability() => _isInvulnerability = true;
    public void DeactivateInvulnerability() => _isInvulnerability = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ability"))
        {
            var ability = other.GetComponentInParent<BaseAbility>();
            if (ability) GetHit(Mathf.RoundToInt(ability.Value));
        }
    }

    public virtual void Heal(int value)
    {
        if (_isDied) return;

        if (healths + value > maxHealth) value = maxHealth;

        healths += value;
    }

    public virtual void GetHit(int damage)
    {
        if (_isDied || _isInvulnerability) return;

        damage -= resistance;
        if (damage <= 0) damage = 1;

        healths -= damage;
        
        DamageCountText.Create(this, damage);
    }

    protected virtual void Die()
    {
        _collider.enabled = false;
        _isDied = true;
    }
}