using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DashSystem : BaseSystem, ILockayable
{
    [SerializeField] private float distance, duration, reloadDuration;
    [SerializeField] private LayerMask unitLayerMask;
    [SerializeField] private UnityEvent onStartDash, onEndDash;
    private LockayablesManager lockayablesManager;

    private bool _isDashing, _isReloading;
    public bool IsDashing => _isDashing;

    public bool IsLocked { get; set; }

    private Collider _collider;

    public override void Awake()
    {
        base.Awake();
        _collider = GetComponent<Collider>();
    }

    public override void Upgrade(float value)
    {
        distance += value;
    }

    private void Start()
    {
        lockayablesManager = new LockayablesManager(this);

        onStartDash.AddListener(lockayablesManager.LockAll);
        onEndDash.AddListener(lockayablesManager.UnlockAll);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            ForceDash();
        }
    }
    public void ForceDash()
    {
        if (CanDash == false) return;

        StartCoroutine(Dashing());
    }

    private IEnumerator Dashing()
    {
        float speed = distance / duration;
        float savedAnimatorSpeed = _animator.speed;
        float savedDrag = _rigidbody.drag;

        onStartDash.Invoke();
        _isDashing = true;

        _animator.SetTrigger("Dash");
        _animator.speed = savedAnimatorSpeed / duration;

        _collider.excludeLayers = unitLayerMask;

        _rigidbody.drag = 0;
        _rigidbody.velocity = speed * transform.forward;
        yield return new WaitForSeconds(duration);
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        StartCoroutine(Reloading());

        _rigidbody.drag = savedDrag;
        _animator.speed = savedAnimatorSpeed;
        _collider.excludeLayers = 0;
        _isDashing = false;

        onEndDash.Invoke();
        yield break;
    }
    private IEnumerator Reloading()
    {
        _isReloading = true;
        yield return new WaitForSeconds(reloadDuration);
        _isReloading = false;
        yield break;
    }
    private bool CanDash => _isDashing == false && _isReloading == false && IsLocked == false;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.localPosition, transform.forward * distance);
    }
}