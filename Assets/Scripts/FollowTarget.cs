using UnityEngine;
public class FollowTarget : BaseSystem, ILockayable
{
    [SerializeField, Range(0f, 20f)] private float movementSpeed;
    [SerializeField] private Transform target;
    private MoveManager _moveManager;
    private Vector3 _direction;

    private float _attackRange;
    public bool IsLocked { get; set; }
    private void Start()
    {
        if (target == null) throw new System.Exception("Target not founded");

        _attackRange = TryGetComponent(out AttackSystem attackSystem) ? attackSystem.RadiusCurrentAttackUpgrade : 0.5f;
        if (target.TryGetComponent(out HealthSystem targetHealthSystem)) targetHealthSystem.OnHealthsOver.AddListener(Lock);
        _moveManager = new MoveManager(_rigidbody, movementSpeed, _attackRange);
    }
    public void Initialize(Transform target)
    {
        this.target = target;
    }
    private void Update()
    {
        if (target is null || IsLocked) return;

        _direction = target.position - transform.position;
        _animator.SetBool(_moveManager.AnimatorNameParametr, _moveManager.IsMoving(_direction));
    }
    private void FixedUpdate()
    {
        if (IsLocked) return;

        _moveManager.Rotate(_direction);
        _moveManager.Move(_direction);
    }

    public void Lock()
    {
        IsLocked = true;
        _animator.SetBool(_moveManager.AnimatorNameParametr, false);
    }
}