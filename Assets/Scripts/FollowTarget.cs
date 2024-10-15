using UnityEngine;
public class FollowTarget : BaseSystem, ILockayable
{
    [SerializeField, Range(0f, 20f)] private float movementSpeed;
    [SerializeField] private Transform target;
    private MoveManager moveManager;
    private Vector3 direction;

    private float attackRange;
    public bool IsLocked { get; set; }
    private void Start()
    {
        if (target == null) throw new System.Exception("Target not founded");

        attackRange = TryGetComponent(out AttackSystem attackSystem) ? attackSystem.Radius : 1;
        if (target.TryGetComponent(out HealthSystem targetHealthSystem)) targetHealthSystem.OnHealthsOver.AddListener(Lock);
        moveManager = new MoveManager(_rigidbody, movementSpeed, attackRange);
    }
    public void Initialize(Transform target)
    {
        this.target = target;
    }
    private void Update()
    {
        if (target is null || IsLocked) return;

        direction = target.position - transform.position;
        _animator.SetBool(moveManager.AnimatorNameParametr, moveManager.IsMoving(direction));
    }
    private void FixedUpdate()
    {
        if (IsLocked) return;

        moveManager.Rotate(direction);
        moveManager.Move(direction);
    }

    public void Lock()
    {
        IsLocked = true;
        _animator.SetBool(moveManager.AnimatorNameParametr, false);
    }

    public override void Upgrade(float value)
    {
        
    }
}